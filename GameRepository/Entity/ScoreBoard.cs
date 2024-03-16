using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameExtensions.Extensions;
using GameRepository.Type;

namespace GameRepository.Entity
{
    [Table("tb_score_board")]
    public class ScoreBoard : BaseEntity
    {
        [Key]
        [Column("idx")]
        public long Idx { get; set; }

        [Column("week_id")]
        public int WeekId { get; set; }

        [Column("mode")]
        public string GameModeString
        {
            get => GameMode.ToString();
            private set => GameMode = value.ParseEnum<GameMode>();
        }

        [NotMapped]
        public GameMode GameMode { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("score")]
        public int Score { get; set; }

        public override string ToString() => this.GetPropertiesAsText();
    }
}