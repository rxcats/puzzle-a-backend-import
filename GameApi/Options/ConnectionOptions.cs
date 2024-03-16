namespace GameApi.Options
{
    public class DatabaseConnectionOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string Version { get; set; }
    }

    public class RedisConnectionOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }
}