using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;

namespace Domain.Entities
{
    public class WalletTransaction : BaseEntity<Wallet, Guid>
    {
        public Guid WalletId { get; set; }

        public Guid TransactonId { get; set; }

        public virtual Wallet Wallet { get; set; }

        public virtual Transaction Transaction { get; set; }

        public TransDirection Type { get; set; }
    }
}
