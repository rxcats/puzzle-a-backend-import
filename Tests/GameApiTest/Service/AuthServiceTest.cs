using System;
using GameApi.Domain;
using GameApi.Service;
using GameRedis.Session;
using GameRepository.Entity;
using GameRepository.Repos;
using Moq;
using NUnit.Framework;

namespace GameApiTest.Service
{
    [TestFixture]
    public class AuthServiceTest
    {
        private Mock<ISessionProvider> _sessionProvider;
        private Mock<IUserInfoRepository> _userInfoRepository;
        private AuthService _authService;

        private readonly UserInfo _testUser = new()
        {
            UserId = 1,
            UserPlatformId = "test.1",
            Nickname = "test.1",
            PhotoUrl = string.Empty,
            ProviderId = string.Empty,
            ProviderUserId = string.Empty,
            ProviderName = string.Empty,
            ProviderEmail = string.Empty,
            CreatedDateTime = DateTime.Now,
            UpdatedDateTime = DateTime.Now
        };

        [SetUp]
        public void SetUp()
        {
            _sessionProvider = new Mock<ISessionProvider>();
            _userInfoRepository = new Mock<IUserInfoRepository>();
            _authService = new AuthService(_sessionProvider.Object, _userInfoRepository.Object);
        }

        [Test]
        public void SignInUser()
        {
            _userInfoRepository.Setup(r => r.FindByUserPlatformId(It.IsAny<string>()))
                .Returns(_testUser);

            _userInfoRepository.Setup(r => r.Update(It.IsAny<UserInfo>()))
                .Returns(_testUser);

            var request = new SignInUserRequest
            {
                UserPlatformId = "test.1",
                AccessToken = "AccessToken",
                Nickname = "test.1",
                PhotoUrl = string.Empty
            };

            var signInUser = _authService.SignInUser(request);

            Assert.AreEqual(1, signInUser.UserId);
            Assert.AreEqual("test.1", signInUser.UserPlatformId);
            Assert.AreEqual("test.1", signInUser.Nickname);
        }
    }
}