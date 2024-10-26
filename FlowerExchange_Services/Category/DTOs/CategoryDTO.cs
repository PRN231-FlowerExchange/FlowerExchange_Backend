using Domain.Constants.Enums;

namespace Application.Category.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public CategoryStatus Status { get; set; }
    }
}
