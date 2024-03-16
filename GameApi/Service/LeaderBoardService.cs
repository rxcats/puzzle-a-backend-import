using System;
using System.Collections.Generic;
using GameApi.Domain;
using GameExtensions.Extensions;
using GameRepository.Entity;
using GameRepository.Repos;
using GameRepository.Type;

namespace GameApi.Service
{
    public class LeaderBoardService
    {
        private readonly IScoreBoardRepository _scoreBoardRepository;

        public LeaderBoardService(IScoreBoardRepository scoreBoardRepository)
        {
            _scoreBoardRepository = scoreBoardRepository;
        }

        public ScoreBoard SaveScore(long userId, int weekId, SaveLeaderBoardScoreRequest request)
        {
            var gameMode = request.GameMode.ParseEnum<GameMode>();

            var score = _scoreBoardRepository.FindUserScore(userId, weekId, request.GameMode.ParseEnum<GameMode>());

            if (score == null)
            {
                score = new ScoreBoard
                {
                    WeekId = weekId,
                    GameMode = gameMode,
                    UserId = userId,
                    Score = request.Score,
                };

                return _scoreBoardRepository.Insert(score);
            }

            score.Score += request.Score;

            return _scoreBoardRepository.Update(score);
        }

        public List<UserScore> GetTopScores(int weekId, LeaderBoardTopScoresRequest request)
        {
            var gameMode = request.GameMode.ParseEnum<GameMode>();

            return _scoreBoardRepository.FindTopScoresWithUserInfo(weekId, gameMode);
        }
    }
}