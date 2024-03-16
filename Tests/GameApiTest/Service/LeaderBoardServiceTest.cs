using System;
using System.Collections.Generic;
using GameApi.Domain;
using GameApi.Service;
using GameRepository.Entity;
using GameRepository.Repos;
using GameRepository.Type;
using Moq;
using NUnit.Framework;

namespace GameApiTest.Service
{
    [TestFixture]
    public class LeaderBoardServiceTest
    {
        private Mock<IScoreBoardRepository> _scoreBoardRepository;
        private LeaderBoardService _leaderBoardService;

        private readonly ScoreBoard _testScoreBoard = new()
        {
            Idx = 1,
            WeekId = 202101,
            GameMode = GameMode.Blast,
            UserId = 1,
            Score = 100,
            CreatedDateTime = DateTime.Now,
            UpdatedDateTime = DateTime.Now
        };

        private readonly UserScore _testUserScore = new()
        {
            UserId = 1,
            UserPlatformId = "1.test",
            Nickname = "1.test",
            PhotoUrl = "",
            WeekId = 202101,
            GameMode = GameMode.Blast.ToString(),
            Score = 100
        };

        [SetUp]
        public void SetUp()
        {
            _scoreBoardRepository = new Mock<IScoreBoardRepository>();
            _leaderBoardService = new LeaderBoardService(_scoreBoardRepository.Object);
        }

        [Test]
        public void SaveScoreInsert()
        {
            _scoreBoardRepository.Setup(r => r.Insert(It.IsAny<ScoreBoard>()))
                .Returns(_testScoreBoard);

            var req = new SaveLeaderBoardScoreRequest
            {
                GameMode = GameMode.Blast.ToString(),
                Score = 100,
            };

            var result = _leaderBoardService.SaveScore(1, 202101, req);

            Assert.AreEqual(100, result.Score);
        }

        [Test]
        public void SaveScoreUpdate()
        {
            _scoreBoardRepository.Setup(r =>
                    r.FindUserScore(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<GameMode>()))
                .Returns(_testScoreBoard);

            _scoreBoardRepository.Setup(r => r.Update(It.IsAny<ScoreBoard>()))
                .Returns(_testScoreBoard);

            var req = new SaveLeaderBoardScoreRequest
            {
                GameMode = GameMode.Blast.ToString(),
                Score = 100,
            };

            var result = _leaderBoardService.SaveScore(1, 202101, req);

            Assert.AreEqual(200, result.Score);
        }

        [Test]
        public void GetTopScores()
        {
            var scores = new List<UserScore> {_testUserScore};

            _scoreBoardRepository.Setup(r =>
                    r.FindTopScoresWithUserInfo(It.IsAny<int>(), It.IsAny<GameMode>(), It.IsAny<int>()))
                .Returns(scores);

            var req = new LeaderBoardTopScoresRequest
            {
                GameMode = GameMode.Blast.ToString()
            };

            var result = _leaderBoardService.GetTopScores(1, req);

            Assert.AreEqual(1, result.Count);
        }
    }
}