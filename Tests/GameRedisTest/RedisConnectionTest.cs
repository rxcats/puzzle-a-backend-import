using NUnit.Framework;

namespace GameRedisTest
{
    [TestFixture]
    public class RedisConnectionTest
    {
        [Test]
        public void Connect()
        {
            using var connection = TestRedisOptions.GetClient().Multiplexer;
            Assert.IsTrue(connection.IsConnected);
        }
    }
}