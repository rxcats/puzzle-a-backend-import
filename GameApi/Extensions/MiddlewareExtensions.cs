using GameApi.Middleware;
using Microsoft.AspNetCore.Builder;

namespace GameApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJsonApiLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsonApiLoggingMiddleware>();
        }

        public static IApplicationBuilder UseMessagePackApiLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MessagePackApiLoggingMiddleware>();
        }
    }
}