using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.DTOs
{
    public class AllPostDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public string Location { get; set; }

        public DateTime ExpiredAt { get; set; }

        public PostStatus PostStatus { get; set; }

        //public List<string> ImageUrls { get; set; } = new List<string>();

        public string UnitMeasure { get; set; }

        public string MainImageUrl { get; set; }

        //public Guid? SellerId { get; set; }

        //public Guid? StoreId { get; set; }

        public StoreDTO Store { get; set; }

        public FlowerDTO Flower { get; set; }
    }
}
