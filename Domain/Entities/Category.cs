using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;

namespace Domain.Entities
{
    public class Category : BaseEntity<Category, Guid>
    {
        public string Name { get; set; }

        public CategoryStatus Status { get; set; }

        public virtual ICollection<PostCategory>? PostCategories { get; set; }
    }
}
