using Domain.Constants.Enums;

namespace Application.PostFlower.DTOs
{
    public class PostDTO
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

        public string MainImageUrl { get; set; }

        //public virtual ICollection<PostCategory> PostCategories { get; set; }

        //public virtual ICollection<Domain.Entities.Report> Reports { get; set; }

        public Guid? SellerId { get; set; }

        //public virtual User? Seller { get; set; }

        public Guid? StoreId { get; set; }

        //public virtual StoreDTO? Store { get; set; }

        //public virtual Flower Flower { get; set; }

        //public virtual ICollection<PostService>? PostServices { get; set; }
        public StoreDTO Store { get; set; }
        public FlowerDTO Flower { get; set; }
        public SellerDTO Seller { get; set; }
    }
}
