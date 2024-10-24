using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;

namespace Domain.Entities
{
    public class Post : BaseEntity<Post, Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public string Location { get; set; }

        public DateTime ExpiredAt { get; set; }

        public PostStatus PostStatus { get; set; } //tinh trang bai post góc nhìn admin bài post được activated

        public List<string> ImageUrls { get; set; } = new List<string>();

        public string UnitMeasure { get; set; }

        public string MainImageUrl { get; set; }

        public virtual ICollection<PostCategory> PostCategories { get; set; } //tập trung các category mà hoa của bài post thuộc về

        public virtual ICollection<Report> Reports { get; set; }

        public Guid? SellerId { get; set; }

        public virtual User? Seller { get; set; }

        public Guid? StoreId { get; set; }

        public virtual Store? Store { get; set; }

        public virtual Flower Flower { get; set; }

        public virtual ICollection<PostService>? PostServices { get; set; }

    }
}
