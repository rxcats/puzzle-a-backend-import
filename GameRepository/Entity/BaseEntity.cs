using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameRepository.Entity
{
    public class BaseEntity
    {
        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }

        [Column("updated_datetime")]
        public DateTime UpdatedDateTime { get; set; }
    }
}