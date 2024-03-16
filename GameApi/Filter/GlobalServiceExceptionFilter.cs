using System.Net;
using System.Net.Mime;
using GameApi.Domain;
using GameApi.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace GameApi.Filter
{
    public class GlobalServiceExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalServiceExceptionFilter> _logger;

        public GlobalServiceExceptionFilter(ILogger<GlobalServiceExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.ZLogError("{0}", context.Exception.Message);
            _logger.ZLogError("{0}", context.Exception.StackTrace);

            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            switch (context.HttpContext.Request.ContentType)
            {
                case MediaType.MessagePack:
                {
                    context.HttpContext.Response.ContentType = MediaType.MessagePack;
                    CreateErrorResult(context);
                    break;
                }
                case MediaTypeNames.Application.Json:
                {
                    context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    CreateErrorResult(context);
                    break;
                }
                default:
                {
                    context.HttpContext.Response.ContentType = MediaTypeNames.Text.Plain;
                    CreateTextErrorResult(context);
                    break;
                }
            }
        }

        private void CreateErrorResult(ExceptionContext context)
        {
            var code = (int) ResultCode.InternalServerError;

            if (context.Exception is ServiceException)
            {
                var e = (ServiceException) context.Exception;
                code = (int) e.GetResultCode();
            }

            context.Result = new ObjectResult(new ApiResponse<string>
            {
                Code = code,
                Result = context.Exception.Message
            });
        }

        private void CreateTextErrorResult(ExceptionContext context)
        {
            context.Result = new ObjectResult(context.Exception.Message);
        }
    }
}