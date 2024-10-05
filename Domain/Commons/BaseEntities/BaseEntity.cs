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
    public abstract class BaseEntity<TEntity, TKey> : ITrackable, IEntityWitkKey<TKey>
    {
        //Domain Key
        [Key]
        public TKey Id { get; set; } = default!;

        //Domain Auditatable
        [Timestamp]
        public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual DateTimeOffset? CreatedAt { get; set; }

        public virtual DateTimeOffset? UpdatedAt { get; set; }

        public virtual Guid? createById { get; set; }    

        public virtual Guid? updateById { get; set; }

        [NotMapped]
        public virtual User? CreatedBy { get; set; }

        [NotMapped]
        public virtual User? UpdatedBy { get; set; }
    }
}
