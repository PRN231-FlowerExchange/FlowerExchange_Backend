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
    [Table("FlowerOrder")]
    public class FlowerOrder
    {
        [Key]
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public bool IsRefund { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
        
        public Guid BuyerId { get; set; }

        public User Buyer { get; set; }

        public Guid SellerId { get; set; }

        public User Seller { get; set; }

        public Guid FlowerId { get; set; }

        public Flower Flower { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }


    }
}
