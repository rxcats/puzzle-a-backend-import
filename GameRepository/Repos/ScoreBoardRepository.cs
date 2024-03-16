using System.Collections.Generic;
using System.Linq;
using GameRepository.Entity;
using GameRepository.Type;
using Microsoft.EntityFrameworkCore;

namespace GameRepository.Repos
{
    public interface IScoreBoardRepository
    {
        public ScoreBoard FindUserScore(long userId, int weekId, GameMode mode);
        public List<ScoreBoard> FindTopScores(int weekId, GameMode mode, int limit = 100);
        public List<UserScore> FindTopScoresWithUserInfo(int weekId, GameMode mode, int limit = 100);
        public ScoreBoard Insert(ScoreBoard scoreBoard);
        public ScoreBoard Update(ScoreBoard scoreBoard);
        public int DeleteUserWeekScore(long userId, int weekId, GameMode mode);
    }

    public class ScoreBoardRepository : IScoreBoardRepository
    {
        private readonly IDbContextFactory<GameDatabaseContext> _dbContextFactory;

        public ScoreBoardRepository(IDbContextFactory<GameDatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public ScoreBoard FindUserScore(long userId, int weekId, GameMode mode)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            return ctx.ScoreBoard
                .SingleOrDefault(score =>
                    score.WeekId == weekId &&
                    score.GameModeString == mode.ToString() &&
                    score.UserId == userId);
        }

        public List<ScoreBoard> FindTopScores(int weekId, GameMode mode, int limit = 100)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            return ctx.ScoreBoard
                .Where(score => score.WeekId == weekId && score.GameModeString == mode.ToString())
                .OrderByDescending(score => score.Score)
                .Take(limit)
                .ToList();
        }

        public List<UserScore> FindTopScoresWithUserInfo(int weekId, GameMode mode, int limit = 100)
        {
            using var ctx = _dbContextFactory.CreateDbContext();

            return ctx.ScoreBoard
                .Where(score => score.WeekId == weekId && score.GameModeString == mode.ToString())
                .OrderByDescending(score => score.Score)
                .Take(limit)
                .Join(ctx.UserInfo,
                    scoreBoard => scoreBoard.UserId,
                    userInfo => userInfo.UserId,
                    (s, u) => new UserScore
                    {
                        UserId = u.UserId,
                        UserPlatformId = u.UserPlatformId,
                        Nickname = u.Nickname,
                        PhotoUrl = u.PhotoUrl,
                        WeekId = s.WeekId,
                        GameMode = s.GameModeString,
                        Score = s.Score
                    })
                .ToList();
        }

        public ScoreBoard Insert(ScoreBoard scoreBoard)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var entity = ctx.ScoreBoard.Add(scoreBoard)
                .Entity;
            ctx.SaveChanges();
            return entity;
        }

        public ScoreBoard Update(ScoreBoard scoreBoard)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var entity = ctx.ScoreBoard.Update(scoreBoard)
                .Entity;
            ctx.SaveChanges();
            return entity;
        }

        public int DeleteUserWeekScore(long userId, int weekId, GameMode mode)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var entity = ctx.ScoreBoard
                .SingleOrDefault(score =>
                    score.WeekId == weekId &&
                    score.GameModeString == mode.ToString() &&
                    score.UserId == userId);
            if (entity == null) return 0;
            ctx.ScoreBoard.Remove(entity);
            return ctx.SaveChanges();
        }
    }
}