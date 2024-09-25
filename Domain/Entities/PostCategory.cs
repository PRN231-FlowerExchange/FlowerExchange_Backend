using Domain.Commons.BaseEntities;


namespace Domain.Entities
{
    public class PostCategory : BaseEntity<PostCategory, Guid>
    {
        public Guid PostId { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Post Post { get; set; }

        public virtual Category Category { get; set; }
    }
}
