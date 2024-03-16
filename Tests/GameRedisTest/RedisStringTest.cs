using System;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using StackExchange.Redis;

namespace GameRedisTest
{
    [TestFixture]
    public class RedisStringTest
    {
        private record Person(long Id, string Name, double Number);

        private IDatabase _client;

        [SetUp]
        public void SetUp()
        {
            _client = TestRedisOptions.GetClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Multiplexer.Dispose();
        }

        [Test]
        public void GetAndSet()
        {
            var alice = new Person(1, "Alice", 0.1);
            var aliceJson = JsonConvert.SerializeObject(alice);

            _client.StringSet("person:1", aliceJson, TimeSpan.FromMilliseconds(100));
            var aliceValue = _client.StringGet("person:1");
            Assert.IsTrue(aliceValue.HasValue);

            var aliceObject = JsonConvert.DeserializeObject<Person>(aliceValue);
            Assert.AreEqual(alice, aliceObject);

            // Wait For Expire
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

            aliceValue = _client.StringGet("person:1");
            Assert.IsFalse(aliceValue.HasValue);
        }
    }
}