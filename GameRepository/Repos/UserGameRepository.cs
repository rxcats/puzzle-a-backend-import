using System.Linq;
using GameRepository.Entity;
using Microsoft.EntityFrameworkCore;

namespace GameRepository.Repos
{
    public interface IUserGameRepository
    {
        public UserGame FindByUserId(long userId);
        public UserGame Insert(UserGame userGame);
        public UserGame Update(UserGame userGame);
        public int DeleteByUserId(long userId);
    }

    public class UserGameRepository : IUserGameRepository
    {
        private readonly IDbContextFactory<GameDatabaseContext> _dbContextFactory;

        public UserGameRepository(IDbContextFactory<GameDatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public UserGame FindByUserId(long userId)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            return ctx.UserGame
                .SingleOrDefault(game => game.UserId == userId);
        }

        public UserGame Insert(UserGame userGame)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var entity = ctx.UserGame.Add(userGame)
                .Entity;
            ctx.SaveChanges();
            return entity;
        }

        public UserGame Update(UserGame userGame)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var entity = ctx.UserGame.Update(userGame)
                .Entity;
            ctx.SaveChanges();
            return entity;
        }

        public int DeleteByUserId(long userId)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var userGame = ctx.UserGame
                .SingleOrDefault(g => g.UserId == userId);
            if (userGame == null) return 0;
            ctx.UserGame.Remove(userGame);
            return ctx.SaveChanges();
        }
    }
}