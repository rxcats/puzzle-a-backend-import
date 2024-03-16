using GameRepository;
using GameRepository.Entity;
using GameRepository.Repos;
using NUnit.Framework;

namespace GameRepositoryTest.Repos
{
    [TestFixture]
    public class UserGameRepositoryTest
    {
        private IUserGameRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = TestDbOptions.Create();

            _repository = new UserGameRepository(new GameDatabaseContextFactory(options));
        }

        [Test]
        public void FindByUserId()
        {
            _repository.DeleteByUserId(1);

            _repository.Insert(new UserGame
            {
                UserId = 1,
                Heart = 0,
                PlayCount = 1
            });

            var game = _repository.FindByUserId(1);

            Assert.AreEqual(1, game.UserId);
            Assert.AreEqual(1, game.PlayCount);
        }

        [Test]
        public void Insert()
        {
            _repository.DeleteByUserId(1);

            var game = _repository.Insert(new UserGame
            {
                UserId = 1,
                Heart = 0,
                PlayCount = 1
            });

            Assert.AreEqual(1, game.UserId);
            Assert.AreEqual(1, game.PlayCount);
        }

        [Test]
        public void Update()
        {
            _repository.DeleteByUserId(1);

            var game = _repository.Insert(new UserGame
            {
                UserId = 1,
                Heart = 0,
                PlayCount = 1
            });

            game.PlayCount = 2;

            game = _repository.Update(game);

            Assert.AreEqual(1, game.UserId);
            Assert.AreEqual(2, game.PlayCount);
        }

        [Test]
        public void DeleteByUserId()
        {
            _repository.DeleteByUserId(1);

            _repository.Insert(new UserGame
            {
                UserId = 1,
                Heart = 0,
                PlayCount = 1
            });

            var result = _repository.DeleteByUserId(1);

            Assert.IsTrue(result == 1);
        }
    }
}