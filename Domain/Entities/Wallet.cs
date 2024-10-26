using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;

namespace Domain.Entities
{
    public class Wallet : BaseEntity<Wallet, Guid>
    {
        public double TotalBalance { get; set; }

        public Currency Currency { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<DepositTransaction>? DepositTransactions { get; set; }

        public virtual ICollection<WalletTransaction>? WalletTransactions { get; set; }
    }
}
