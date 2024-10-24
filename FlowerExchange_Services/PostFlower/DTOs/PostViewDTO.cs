using Application.Report.DTOs;
using Domain.Constants.Enums;

namespace Application.PostFlower.DTOs
{
    public class PostViewDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public string Location { get; set; }

        public DateTime ExpiredAt { get; set; }

        public PostStatus PostStatus { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();

        public string UnitMeasure { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public string MainImageUrl { get; set; }

        public List<PostCategoryDTO> Categories { get; set; }

        public List<ReportModel> Reports { get; set; }

        public Guid SellerId { get; set; }

        //public UserModel? Seller { get; set; }

        public Guid StoreId { get; set; }

        //public StoreModel? Store { get; set; }

        //public FlowerModel? Flower { get; set; }

        //public List<PostServiceModel>? PostServices { get; set; }

        public int priority { get; set; } = 0;
    }
}
