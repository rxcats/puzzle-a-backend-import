using GameExtensions.Extensions;

namespace GameRepository.Entity
{
    public class UserScore
    {
        public long UserId { get; set; }

        public string UserPlatformId { get; set; }

        public string Nickname { get; set; }

        public string PhotoUrl { get; set; }

        public int WeekId { get; set; }

        public string GameMode { get; set; }

        public int Score { get; set; }

        public override string ToString() => this.GetPropertiesAsText();
    }
}