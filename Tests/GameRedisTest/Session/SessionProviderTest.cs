using GameRedis.Session;
using NUnit.Framework;
using StackExchange.Redis;

namespace GameRedisTest.Session
{
    [TestFixture]
    public class SessionProviderTest
    {
        private ISessionProvider _provider;
        private IDatabase _client;

        [SetUp]
        public void SetUp()
        {
            _client = TestRedisOptions.GetClient();
            _provider = new SessionProvider(_client);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Multiplexer.Dispose();
        }

        [Test]
        public void SetGet()
        {
            var session = new UserSession
            {
                UserId = 1,
                AccessToken = "AccessToken",
            };
            session.SetLoginDateTime();

            _provider.Set(session);

            var user = _provider.Get(session.UserId);
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user?.UserId);
            Assert.AreEqual("AccessToken", user?.AccessToken);
        }

        [Test]
        public void Remove()
        {
            var session = new UserSession
            {
                UserId = 1,
                AccessToken = "AccessToken",
            };
            session.SetLoginDateTime();

            _provider.Set(session);
            _provider.Remove(session.UserId);

            var user = _provider.Get(session.UserId);
            Assert.IsNull(user);
        }
    }
}