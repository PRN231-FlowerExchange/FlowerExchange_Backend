using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commons.BaseEntities
{
    public abstract class BaseEntity<TEntity, TKey> : ITrackable    {
        //Domain Key
        [Key]
        public TKey Id { get; set; } = default!;

        //Domain Auditatable
        [Timestamp]
        [NotMapped]
        public byte[] RowVersion { get; set; } = default!;

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public Guid? createById { get; set; }    

        public Guid? updateById { get; set; }

        [NotMapped]
        public User? CreatedBy { get; set; }

        [NotMapped]
        public User? UpdatedBy { get; set; }
    }
}
