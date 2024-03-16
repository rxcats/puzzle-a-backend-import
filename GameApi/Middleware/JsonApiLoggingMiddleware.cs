using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using GameApi.Domain;
using GameApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using ZLogger;

namespace GameApi.Middleware
{
    public class JsonApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JsonApiLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public JsonApiLoggingMiddleware(RequestDelegate next, ILogger<JsonApiLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        private bool ShouldNotFilter(HttpContext context)
        {
            return context.Request.ContentType != MediaTypeNames.Application.Json;
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
            var readChunk = new char[readChunkBufferLength];
            using var reader = new StreamReader(stream);
            await using var stringWriter = new StringWriter();
            int readChunkLength;
            do
            {
                readChunkLength = await reader.ReadBlockAsync(readChunk, 0, readChunkBufferLength);
                await stringWriter.WriteAsync(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return await Task.FromResult(stringWriter.ToString());
        }

        private async Task<string> ReadResponseBody(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseStream = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            await responseStream.CopyToAsync(originalBodyStream);

            return await Task.FromResult(body);
        }
    }
}