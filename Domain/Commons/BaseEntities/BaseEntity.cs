using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        private readonly List<BaseEvent> _domainEvents = new();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
