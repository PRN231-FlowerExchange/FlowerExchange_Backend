using Domain.Commons.BaseEntities;

namespace Domain.Entities
{
    public class Store : BaseEntity<Store, Guid>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string? CoverPhotoUrl { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Descriptions { get; set; }

        public string? Slug { get; set; }

        public IList<string>? Phones { get; set; }

        public Guid OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }
    }
}
