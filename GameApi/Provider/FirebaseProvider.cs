using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using GameApi.Domain;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GameAppOptions = GameApi.Options.AppOptions;

namespace GameApi.Provider
{
    public class FirebaseProvider
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly FirebaseAuth _firebaseAuth;
        private readonly FirebaseMessaging _firebaseMessaging;
        private readonly GameAppOptions _gameAppOptions;
        private readonly ILogger<FirebaseProvider> _logger;

        public FirebaseProvider(IConfiguration configuration, ILogger<FirebaseProvider> logger)
        {
            _logger = logger;
            _gameAppOptions = new GameAppOptions();
            configuration.GetSection("AppOptions").Bind(_gameAppOptions);

            _firebaseApp = FirebaseApp.Create(GetOptions());
            _firebaseAuth = FirebaseAuth.GetAuth(_firebaseApp);
            _firebaseMessaging = FirebaseMessaging.GetMessaging(_firebaseApp);
        }

        private AppOptions GetOptions()
        {
            var credential = GoogleCredential.FromFile(_gameAppOptions.FirebaseCredentialFile);
            var options = new AppOptions
            {
                Credential = credential
            };

            return options;
        }

        public FirebaseApp GetFirebaseApp()
        {
            return _firebaseApp;
        }

        public FirebaseAuth GetFirebaseAuth()
        {
            return _firebaseAuth;
        }

        public FirebaseMessaging GetFirebaseMessaging()
        {
            return _firebaseMessaging;
        }

        public void ValidateAccessToken(string userPlatformId, string accessToken)
        {
            try
            {
                var decoded = _firebaseAuth.VerifyIdTokenAsync(accessToken).Result;

                if (decoded.Uid != userPlatformId)
                {
                    throw new ServiceException(ResultCode.InvalidAccessToken, "Firebase VerifyIdToken Uid Not Equals.");
                }
            }
            catch
            {
                throw new ServiceException(ResultCode.InvalidAccessToken, "Firebase VerifyIdToken Failure.");
            }
        }
    }
}