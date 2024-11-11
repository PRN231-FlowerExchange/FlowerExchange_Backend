using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class FlowerOrder : BaseEntity<FlowerOrder, Guid>
    {
        public double Amount { get; set; }

        public bool IsRefund { get; set; }

        public virtual OrderStatus Status { get; set; }

        public Guid BuyerId { get; set; }

        public virtual User Buyer { get; set; }

        public Guid SellerId { get; set; }

        public virtual User Seller { get; set; }

        public Guid FlowerId { get; set; }

        public virtual Flower Flower { get; set; }
        
        public virtual ICollection<Transaction>? Transactions { get; set; }


    }
}
