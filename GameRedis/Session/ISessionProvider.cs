namespace GameRedis.Session
{
    public interface ISessionProvider
    {
        UserSession? Get(long userId);

        void Set(UserSession userSession);

        void Remove(long userId);
    }
}