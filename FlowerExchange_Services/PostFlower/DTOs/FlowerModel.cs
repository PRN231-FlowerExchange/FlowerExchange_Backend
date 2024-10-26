using Application.Order.DTOs;
using Domain.Constants.Enums;

namespace Application.PostFlower.DTOs
{
    public class FlowerModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public DateTime CreateAt { get; set; }

        //public Guid CreateBy { get; set; }

        public DateTime UpdateAt { get; set; }

        //public Guid UpdateBy { get; set; }

        public Currency Currency { get; set; }

        public Guid PostId { get; set; }

        public PostModel Post { get; set; }

        public FlowerOrderModel FlowerOrder { get; set; }


    }
}
