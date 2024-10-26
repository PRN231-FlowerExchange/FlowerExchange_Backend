using Application.PaymentTransaction.DTOs;
using Domain.Constants.Enums;

namespace Application.UserWallet.DTOs
{
    public class WalletTransactionModel
    {
        public Guid WalletId { get; set; }

        public WalletModel Wallet { get; set; }

        public Guid TransactonId { get; set; }

        public TransactionModel Transaction { get; set; }

        public TransDirection Type { get; set; }
    }
}
