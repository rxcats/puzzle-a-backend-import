using GameRepository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GameRepositoryTest
{
    [TestFixture]
    public class GameDatabaseContextTest
    {
        private DbContextOptions<GameDatabaseContext> _options;

        [SetUp]
        public void SetUp()
        {
            _options = TestDbOptions.Create();
        }

        [Test]
        public void LoadContext()
        {
            using var context = new GameDatabaseContext(_options);
            Assert.IsNotNull(context);
        }
    }
}