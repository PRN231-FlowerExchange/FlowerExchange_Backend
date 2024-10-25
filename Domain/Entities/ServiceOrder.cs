using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;

namespace Domain.Entities
{

    public class ServiceOrder : BaseEntity<ServiceOrder, Guid>
    {
        public double Amount { get; set; }

        public OrderStatus Status { get; set; }

        public Guid BuyerId { get; set; }

        public virtual User Buyer { get; set; }

        public virtual ICollection<PostService>? PostServices { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
