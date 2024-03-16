using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameRedis.Session
{
    public class UserSession
    {
        public long UserId { get; set; }

        public string AccessToken { get; set; }

        public DateTime LoginDateTime { get; set; }

        public DateTime LastAccessTime { get; set; }

        public void SetLoginDateTime()
        {
            LoginDateTime = DateTime.Now;
            LastAccessTime = DateTime.Now;
        }

        public void SetLastAccessDateTime()
        {
            LastAccessTime = DateTime.Now;
        }
    }

    public class SessionProvider : ISessionProvider
    {
        private const string SessionKeyPrefix = "session:{0}";
        private readonly TimeSpan _sessionDefaultTimeOut = TimeSpan.FromDays(7);

        private readonly IDatabase _database;

        public SessionProvider(IDatabase database)
        {
            _database = database;
        }

        public UserSession? Get(long userId)
        {
            var redisValue = _database.StringGet(string.Format(SessionKeyPrefix, userId));
            return redisValue.HasValue ? JsonConvert.DeserializeObject<UserSession>(redisValue) : null;
        }

        public void Set(UserSession userSession)
        {
            var json = JsonConvert.SerializeObject(userSession);
            _database.StringSet(string.Format(SessionKeyPrefix, userSession.UserId), json, _sessionDefaultTimeOut);
        }

        public void Remove(long userId)
        {
            _database.KeyDelete(string.Format(SessionKeyPrefix, userId));
        }
    }
}