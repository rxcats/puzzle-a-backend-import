using System;
using System.Linq;
using GameApi.Domain;
using GameApi.Options;
using GameRedis.Session;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace GameApi.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateAccessTokenAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ValidateAccessTokenAttribute> _logger;
        private readonly ISessionProvider _sessionProvider;
        private readonly AppOptions _appOptions;

        public ValidateAccessTokenAttribute(ISessionProvider sessionProvider,
            ILogger<ValidateAccessTokenAttribute> logger, IConfiguration configuration)
        {
            _logger = logger;
            _sessionProvider = sessionProvider;

            _appOptions = new AppOptions();
            configuration.GetSection(nameof(AppOptions)).Bind(_appOptions);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Request.Headers["X-UserId"].FirstOrDefault() ?? string.Empty;

            var accessToken = context.HttpContext.Request.Headers["X-AccessToken"].FirstOrDefault() ?? string.Empty;

            _logger.ZLogInformation("UserId: {0}, AccessToken: {1}", userId, accessToken);

            ValidateAccessToken(ParseUserId(userId), accessToken);
        }

        private long ParseUserId(string headerUserId)
        {
            return string.IsNullOrWhiteSpace(headerUserId) ? 0 : long.Parse(headerUserId);
        }

        private void ValidateAccessToken(long userId, string accessToken)
        {
            if (userId == 0 || accessToken == string.Empty)
            {
                throw new ServiceException(ResultCode.InvalidAccessToken,
                    "Required Headers `X-UserId` And `X-AccessToken`");
            }

            if (!_appOptions.EnableValidateAccessToken) return;

            var session = _sessionProvider.Get(userId)
                          ?? throw new ServiceException(ResultCode.UnAuthorized, "UserSession Could Not Found.");

            if (session.AccessToken != accessToken)
            {
                throw new ServiceException(ResultCode.InvalidAccessToken, "Invalid AccessToken.");
            }

            session.SetLastAccessDateTime();

            _sessionProvider.Set(session);
        }
    }
}