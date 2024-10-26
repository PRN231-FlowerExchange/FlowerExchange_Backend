using Domain.Constants.Enums;

namespace Application.Category.DTOs
{
    public class CategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public CategoryStatus Status { get; set; }

        public DateTime CreateAt { get; set; }

        // public List<PostCategoryModel>? PostCategories { get; set; }
    }
}
