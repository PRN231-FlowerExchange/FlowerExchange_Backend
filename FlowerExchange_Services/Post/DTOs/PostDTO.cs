using Application.Report.DTOs;
using Application.UserStore.DTOs;
using Domain.Constants.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.DTOs
{
    public class PostDTO
    {
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

        public virtual StoreDTO? Store { get; set; }

        //public virtual Flower Flower { get; set; }

        //public virtual ICollection<PostService>? PostServices { get; set; }
    }
}
