using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public string Location { get; set; }

        public DateTime ExpiredAt { get; set;}

        public PostStatus PostStatus { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();

        public string UnitMeasure {  get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public string MainImageUrl { get; set; }

        public ICollection<PostCategory> PostCategories { get; set; }

        public ICollection<Report> Reports { get; set; }

        public Guid SellerId { get; set; }

        public User Seller { get; set; }

        public Guid StoreId { get; set; }

        public Store Store { get; set; }

        public Flower Flower { get; set; }

        public ICollection<PostService>? PostServices { get; set; }

    }
}
