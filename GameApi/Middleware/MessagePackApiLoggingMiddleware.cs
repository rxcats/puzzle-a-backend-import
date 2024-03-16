using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameApi.Domain;
using GameApi.Extensions;
using GameApi.Web;
using MessagePack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using ZLogger;

namespace GameApi.Middleware
{
    public class MessagePackApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MessagePackApiLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public MessagePackApiLoggingMiddleware(RequestDelegate next, ILogger<MessagePackApiLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        private bool ShouldNotFilter(HttpContext context)
        {
            return context.Request.ContentType != MediaType.MessagePack;
        }

        public async Task Invoke(HttpContext context)
        {
            if (ShouldNotFilter(context))
            {
                await _next(context);
                return;
            }

            try
            {
                var data = await ParseLogData(context);
                _logger.ZLogInformation("[{0}] {1}", data.Method, data.Uri);
                _logger.ZLogInformation("Status={0}", data.Status);
                _logger.ZLogInformation("ClientIp={0}", data.ClientIp);
                _logger.ZLogInformation("RequestHeaders={0}", data.RequestHeadersToString());
                _logger.ZLogInformation("RequestBody={0}", data.RequestBody);
                _logger.ZLogInformation("ResponseBody={0}", data.ResponseBody);
            }
            catch (Exception e)
            {
                _logger.ZLogError("{0}", e);
            }
        }

        private async Task<ApiLogData> ParseLogData(HttpContext context)
        {
            var uri = context.Request.QueryString.HasValue
                ? $"{context.Request.Path.ToString()}?{context.Request.QueryString.ToString()}"
                : context.Request.Path.ToString();

            var headers = new Dictionary<string, List<string>>();

            foreach (var header in context.Request.Headers)
            {
                headers[header.Key] = header.Value.ToList();
            }

            var requestBody = await ReadRequestBody(context);
            var responseBody = await ReadResponseBody(context);

            return new ApiLogData
            {
                Method = context.Request.Method,
                Uri = uri,
                Status = context.Response.StatusCode,
                ClientIp = context.GetClientIp(),
                RequestHeaders = headers,
                RequestBody = requestBody,
                ResponseBody = responseBody
            };
        }

        private async Task<string> ReadRequestBody(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            requestStream.Seek(0, SeekOrigin.Begin);
            var body = ReadStreamToString(requestStream);
            context.Request.Body.Position = 0;
            return await body;
        }

        private async Task<string> ReadStreamToString(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            var buffer = new byte[readChunkBufferLength];
            using var reader = new StreamReader(stream);
            await using var memStream = _recyclableMemoryStreamManager.GetStream();
            int readChunkLength;
            do
            {
                readChunkLength = await reader.BaseStream.ReadAsync(buffer.AsMemory(0, readChunkBufferLength));
                memStream.Write(buffer, 0, readChunkLength);
            } while (readChunkLength > 0);

            return await Task.FromResult(MessagePackSerializer.ConvertToJson(memStream.ToArray()));
        }

        private async Task<string> ReadResponseBody(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseStream = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await using var cloneStream = _recyclableMemoryStreamManager.GetStream();
            await context.Response.Body.CopyToAsync(cloneStream);

            cloneStream.Seek(0, SeekOrigin.Begin);
            var body = ReadStreamToString(cloneStream);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseStream.CopyToAsync(originalBodyStream);

            return await body;
        }
    }
}