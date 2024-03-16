using GameRepository;
using GameRepository.Entity;
using GameRepository.Repos;
using NUnit.Framework;

namespace GameRepositoryTest.Repos
{
    [TestFixture]
    public class UserInfoRepositoryTest
    {
        private IUserInfoRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = TestDbOptions.Create();

            _repository = new UserInfoRepository(new GameDatabaseContextFactory(options));
        }

        [Test]
        public void FindByUserPlatformId()
        {
            _repository.DeleteByUserPlatformId("test.1");

            _repository.Insert(new UserInfo
            {
                UserPlatformId = "test.1"
            });

            var user = _repository.FindByUserPlatformId("test.1");

            Assert.Greater(user.UserId, 0);
        }

        [Test]
        public void FindByUserId()
        {
            _repository.DeleteByUserPlatformId("test.1");

            var u = _repository.Insert(new UserInfo
            {
                UserPlatformId = "test.1"
            });

            var user = _repository.FindByUserId(u.UserId);

            Assert.Greater(user.UserId, 0);
        }

        [Test]
        public void Insert()
        {
            _repository.DeleteByUserPlatformId("test.1");

            var user = _repository.Insert(new UserInfo
            {
                UserPlatformId = "test.1"
            });

            Assert.Greater(user.UserId, 0);
        }

        [Test]
        public void Update()
        {
            _repository.DeleteByUserPlatformId("test.1");

            var user = _repository.Insert(new UserInfo
            {
                UserPlatformId = "test.1"
            });

            user.Nickname = "test.1";

            var updated = _repository.Update(user);

            Assert.AreEqual("test.1", updated.Nickname);
        }

        [Test]
        public void DeleteByUserPlatformId()
        {
            _repository.DeleteByUserPlatformId("test.1");

            var user = _repository.Insert(new UserInfo
            {
                UserPlatformId = "test.1"
            });

            var result = _repository.DeleteByUserPlatformId(user.UserPlatformId);

            Assert.IsTrue(result == 1);
        }

        [Test]
        public void DeleteByUserId()
        {
            _repository.DeleteByUserPlatformId("test.1");

            var user = _repository.Insert(new UserInfo
            {
                UserPlatformId = "test.1"
            });

            var result = _repository.DeleteByUserId(user.UserId);

            Assert.IsTrue(result == 1);
        }
    }
}