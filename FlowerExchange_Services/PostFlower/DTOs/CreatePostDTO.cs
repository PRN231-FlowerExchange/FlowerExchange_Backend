using Domain.Constants.Enums;

namespace Application.PostFlower.DTOs
{
    public class CreatePostDTO
    {
        public Guid SelectedCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public DateTime ExpiredAt { get; set; }
        //public PostStatus PostStatus { get; set; }
        public List<string> ImageUrls { get; set; }
        public string UnitMeasure { get; set; }
        public string MainImageUrl { get; set; }
        public Guid? SellerId { get; set; }
        public Guid? StoreId { get; set; }

        // Thông tin về Flower cần được tạo cùng với Post
        public FlowerDTO Flower { get; set; }
    }

}
