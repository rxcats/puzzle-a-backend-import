using System.Linq;
using GameRepository.Entity;
using Microsoft.EntityFrameworkCore;

namespace GameRepository.Repos
{
    public interface IUserInfoRepository
    {
        public UserInfo FindByUserId(long userId);
        public UserInfo FindByUserPlatformId(string userPlatformId);
        public UserInfo Insert(UserInfo userInfo);
        public UserInfo Update(UserInfo userInfo);
        public int DeleteByUserId(long userId);
        public int DeleteByUserPlatformId(string userPlatformId);
    }

    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly IDbContextFactory<GameDatabaseContext> _dbContextFactory;

        public UserInfoRepository(IDbContextFactory<GameDatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public UserInfo FindByUserId(long userId)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            return ctx.UserInfo
                .SingleOrDefault(user => user.UserId == userId);
        }

        public UserInfo FindByUserPlatformId(string userPlatformId)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            return ctx.UserInfo
                .SingleOrDefault(user => user.UserPlatformId == userPlatformId);
        }

        public UserInfo Insert(UserInfo userInfo)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var entity = ctx.UserInfo.Add(userInfo)
                .Entity;
            ctx.SaveChanges();
            return entity;
        }

        public UserInfo Update(UserInfo userInfo)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var entity = ctx.UserInfo.Update(userInfo)
                .Entity;
            ctx.SaveChanges();
            return entity;
        }

        public int DeleteByUserId(long userId)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var user = ctx.UserInfo
                .SingleOrDefault(u => u.UserId == userId);
            if (user == null) return 0;
            ctx.UserInfo.Remove(user);
            return ctx.SaveChanges();
        }

        public int DeleteByUserPlatformId(string userPlatformId)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var user = ctx.UserInfo
                .SingleOrDefault(u => u.UserPlatformId == userPlatformId);
            if (user == null) return 0;
            ctx.UserInfo.Remove(user);
            return ctx.SaveChanges();
        }
    }
}