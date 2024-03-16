using System.Collections.Generic;
using GameExtensions.Extensions;
using MessagePack;

namespace GameApi.Domain
{
    [MessagePackObject(true)]
    public class SaveLeaderBoardScoreRequest
    {
        public string GameMode { get; set; }

        public int Score { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }

    [MessagePackObject(true)]
    public class SaveLeaderBoardScoreResponse
    {
        public string GameMode { get; set; }

        public int Score { get; set; }

        public int WeekId { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }

    [MessagePackObject(true)]
    public class LeaderBoardTopScoresRequest
    {
        public string GameMode { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }

    [MessagePackObject(true)]
    public class LeaderBoardProfile
    {
        public long UserId { get; set; }

        public string Nickname { get; set; }

        public string PhotoUrl { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }

    [MessagePackObject(true)]
    public class LeaderBoardScore
    {
        public LeaderBoardProfile User { get; set; }

        public string GameMode { get; set; }

        public int Score { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }

    [MessagePackObject(true)]
    public class LeaderBoardTopScoresResponse
    {
        public List<LeaderBoardScore> Scores { get; set; }

        public int WeekId { get; set; }

        public override string ToString()
        {
            return this.GetPropertiesAsText();
        }
    }
}