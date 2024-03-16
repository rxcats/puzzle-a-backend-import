using StackExchange.Redis;

namespace GameRedisTest
{
    public static class TestRedisOptions
    {
        public static IDatabase GetClient()
        {
            var c = ConnectionMultiplexer.Connect("localhost:6379");
            return c.GetDatabase();
        }
    }
}