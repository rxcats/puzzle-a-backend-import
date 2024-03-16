using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameExtensions.Extensions;

namespace GameRepository.Entity
{
    [Table("tb_user_game")]
    public class UserGame : BaseEntity
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("heart")]
        public int Heart { get; set; }

        [Column("play_cnt")]
        public int PlayCount { get; set; }

        public override string ToString() => this.GetPropertiesAsText();
    }
}