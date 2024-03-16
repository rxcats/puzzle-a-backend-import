using System;
using System.Linq;
using GameApi.Domain;
using GameApi.Filter;
using GameApi.Service;
using GameApi.Web;
using GameExtensions.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GameApi.Controller
{
    [ServiceFilter(typeof(ValidateAccessTokenAttribute))]
    [Consumes(MediaType.MessagePack)]
    [Produces(MediaType.MessagePack)]
    [ApiController]
    [Route("[controller]/[action]")]
    public class LeaderBoardController
    {
        private readonly LeaderBoardService _leaderBoardService;

        public LeaderBoardController(LeaderBoardService leaderBoardService)
        {
            _leaderBoardService = leaderBoardService;
        }

        [HttpPost]
        public ApiResponse<SaveLeaderBoardScoreResponse> SaveScore(
            [FromHeader(Name = "X-UserId")] long userId,
            [FromBody] SaveLeaderBoardScoreRequest request)
        {
            var weekOfYear = DateTime.Now.GetWeekOfYear();
            var score = _leaderBoardService.SaveScore(userId, weekOfYear, request);

            return new ApiResponse<SaveLeaderBoardScoreResponse>
            {
                Result = new SaveLeaderBoardScoreResponse
                {
                    GameMode = score.GameModeString,
                    Score = score.Score,
                    WeekId = weekOfYear
                }
            };
        }

        [HttpPost]
        public ApiResponse<LeaderBoardTopScoresResponse> TopScores([FromBody] LeaderBoardTopScoresRequest request)
        {
            var weekOfYear = DateTime.Now.GetWeekOfYear();

            var topScores = _leaderBoardService.GetTopScores(weekOfYear, request);

            var scores = topScores.Select(score => new LeaderBoardScore
                {
                    User = new LeaderBoardProfile
                    {
                        UserId = score.UserId,
                        Nickname = score.Nickname,
                        PhotoUrl = score.PhotoUrl
                    },
                    GameMode = score.GameMode,
                    Score = score.Score,
                }
            ).ToList();

            return new ApiResponse<LeaderBoardTopScoresResponse>
            {
                Result = new LeaderBoardTopScoresResponse
                {
                    Scores = scores,
                    WeekId = weekOfYear
                }
            };
        }
    }
}