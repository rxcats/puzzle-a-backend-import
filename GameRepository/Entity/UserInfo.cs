using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameExtensions.Extensions;

namespace GameRepository.Entity
{
    [Table("tb_user")]
    public class UserInfo : BaseEntity
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("user_platform_id")]
        public string UserPlatformId { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("photo_url")]
        public string PhotoUrl { get; set; }

        [Column("provider_id")]
        public string ProviderId { get; set; }

        [Column("provider_user_id")]
        public string ProviderUserId { get; set; }

        [Column("provider_name")]
        public string ProviderName { get; set; }

        [Column("provider_email")]
        public string ProviderEmail { get; set; }

        public override string ToString() => this.GetPropertiesAsText();
    }
}