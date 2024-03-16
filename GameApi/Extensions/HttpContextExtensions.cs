using System.Linq;
using Microsoft.AspNetCore.Http;

namespace GameApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClientIp(this HttpContext context)
        {
            return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                   ?? context.Connection.RemoteIpAddress?.ToString()
                   ?? string.Empty;
        }
    }
}