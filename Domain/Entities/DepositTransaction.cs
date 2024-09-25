using System;

namespace Domain.Entities
{
    //[Table("DepositTransaction")]
    public class DepositTransaction
    {
        //[Key]
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public DateTime CreateAt { get; set; }

        public Guid WalletId { get; set; }

        public virtual Wallet Wallet { get; set; }
    }
}
