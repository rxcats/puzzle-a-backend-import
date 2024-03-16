using System;
using GameRepository;
using GameRepository.Entity;
using GameRepository.Repos;
using GameRepository.Type;
using NUnit.Framework;

namespace GameRepositoryTest.Repos
{
    [TestFixture]
    public class ScoreBoardTest
    {
        private IScoreBoardRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = TestDbOptions.Create();

            _repository = new ScoreBoardRepository(new GameDatabaseContextFactory(options));
        }

        [Test]
        public void FindUserScore()
        {
            _repository.DeleteUserWeekScore(1, 202101, GameMode.Blast);

            _repository.Insert(new ScoreBoard
            {
                WeekId = 202101,
                GameMode = GameMode.Blast,
                UserId = 1,
                Score = 100
            });

            var scoreBoard = _repository.FindUserScore(1, 202101, GameMode.Blast);
            Assert.IsNotNull(scoreBoard);
            Assert.AreEqual(202101, scoreBoard.WeekId);
            Assert.AreEqual(1, scoreBoard.UserId);
            Assert.AreEqual(GameMode.Blast, scoreBoard.GameMode);
        }

        [Test]
        public void Insert()
        {
            _repository.DeleteUserWeekScore(1, 202101, GameMode.Blast);

            var userScore = _repository.Insert(new ScoreBoard
            {
                WeekId = 202101,
                GameMode = GameMode.Blast,
                UserId = 1,
                Score = 100
            });

            Assert.Greater(userScore.Idx, 0);
        }

        [Test]
        public void Update()
        {
            _repository.DeleteUserWeekScore(1, 202101, GameMode.Blast);

            var userScore = _repository.Insert(new ScoreBoard
            {
                WeekId = 202101,
                GameMode = GameMode.Blast,
                UserId = 1,
                Score = 100
            });

            userScore.Score = 101;

            var updated = _repository.Update(userScore);

            Assert.AreEqual(101, updated.Score);
        }

        [Test]
        public void DeleteUserWeekScore()
        {
            _repository.DeleteUserWeekScore(1, 202101, GameMode.Blast);

            _repository.Insert(new ScoreBoard
            {
                WeekId = 202101,
                GameMode = GameMode.Blast,
                UserId = 1,
                Score = 100
            });

            var result = _repository.DeleteUserWeekScore(1, 202101, GameMode.Blast);

            Assert.IsTrue(result == 1);
        }

        [Test]
        public void FindTopScores()
        {
            var result = _repository.FindTopScores(202101, GameMode.Blast);

            Assert.GreaterOrEqual(0, result.Count);
        }

        [Test]
        public void FindTopScoresWithUserInfo()
        {
            var result = _repository.FindTopScoresWithUserInfo(202101, GameMode.Blast);

            Assert.GreaterOrEqual(0, result.Count);

            // foreach (var r in result)
            // {
            //     Console.WriteLine(r);
            // }
        }
    }
}