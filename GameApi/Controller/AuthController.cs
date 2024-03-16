using GameApi.Domain;
using GameApi.Provider;
using GameApi.Service;
using GameApi.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameApi.Controller
{
    [Consumes(MediaType.MessagePack)]
    [Produces(MediaType.MessagePack)]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AuthService _authService;
        private readonly FirebaseProvider _firebase;

        public AuthController(ILogger<AuthController> logger, AuthService authService, FirebaseProvider firebase)
        {
            _logger = logger;
            _authService = authService;
            _firebase = firebase;
        }

        [HttpPost]
        public ApiResponse<LoginResponse> SignIn([FromBody] SignInUserRequest request)
        {
            _firebase.ValidateAccessToken(request.UserPlatformId, request.AccessToken);

            var user = _authService.SignInUser(request);

            return new ApiResponse<LoginResponse>
            {
                Result = new LoginResponse
                {
                    UserId = user.UserId
                }
            };
        }
    }
}