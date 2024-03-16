using System.Net.Mime;
using System.Threading.Tasks;
using GameApi.Domain;
using GameApi.Provider;
using GameExtensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace GameApi.Controller
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("[controller]/[action]")]
    public class FirebaseTestController
    {
        private readonly ILogger<FirebaseTestController> _logger;
        private readonly FirebaseProvider _firebaseProvider;

        public FirebaseTestController(ILogger<FirebaseTestController> logger, FirebaseProvider firebaseProvider)
        {
            _logger = logger;
            _firebaseProvider = firebaseProvider;
        }

        [HttpGet]
        public async Task<ApiResponse<string>> GetUser([FromQuery] string uid)
        {
            var user = await _firebaseProvider.GetFirebaseAuth()
                .GetUserAsync(uid);

            _logger.ZLogInformation("uid: {0}", user.GetPropertiesAsText());

            return new ApiResponse<string>
            {
                Result = user.GetPropertiesAsText()
            };
        }
    }
}