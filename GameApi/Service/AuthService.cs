using GameApi.Domain;
using GameRedis.Session;
using GameRepository.Entity;
using GameRepository.Repos;

namespace GameApi.Service
{
    public class AuthService
    {
        private readonly ISessionProvider _sessionProvider;

        private readonly IUserInfoRepository _userInfoRepository;

        public AuthService(ISessionProvider sessionProvider, IUserInfoRepository userInfoRepository)
        {
            _sessionProvider = sessionProvider;
            _userInfoRepository = userInfoRepository;
        }

        private void SaveSession(UserInfo user, string accessToken)
        {
            var session = new UserSession
            {
                UserId = user.UserId,
                AccessToken = accessToken,
            };

            session.SetLoginDateTime();

            _sessionProvider.Set(session);
        }

        private UserInfo CreateUserInfo(SignInUserRequest request)
        {
            var user = new UserInfo
            {
                UserPlatformId = request.UserPlatformId,
                Nickname = request.Nickname ?? string.Empty,
                PhotoUrl = request.PhotoUrl ?? string.Empty,
                ProviderId = request.ProviderId ?? string.Empty,
                ProviderUserId = request.ProviderUserId ?? string.Empty,
                ProviderName = request.ProviderName ?? string.Empty,
                ProviderEmail = request.ProviderEmail ?? string.Empty,
            };

            return _userInfoRepository.Insert(user);
        }

        private UserInfo UpdateUserInfo(UserInfo user, SignInUserRequest request)
        {
            if (!string.IsNullOrEmpty(request.ProviderId) && user.ProviderId != request.ProviderId)
            {
                user.ProviderId = request.ProviderId;
            }

            if (!string.IsNullOrEmpty(request.ProviderUserId) && user.ProviderUserId != request.ProviderUserId)
            {
                user.ProviderUserId = request.ProviderUserId;
            }

            if (!string.IsNullOrEmpty(request.ProviderName) && user.ProviderName != request.ProviderName)
            {
                user.ProviderName = request.ProviderName;
            }

            if (!string.IsNullOrEmpty(request.ProviderEmail) && user.ProviderEmail != request.ProviderEmail)
            {
                user.ProviderEmail = request.ProviderEmail;
            }

            return _userInfoRepository.Update(user);
        }

        public UserInfo SignInUser(SignInUserRequest request)
        {
            var user = _userInfoRepository.FindByUserPlatformId(request.UserPlatformId);

            user = user == null ? CreateUserInfo(request) : UpdateUserInfo(user, request);

            SaveSession(user, request.AccessToken);

            return user;
        }
    }
}