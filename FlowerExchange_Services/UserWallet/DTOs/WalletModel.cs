using Application.UserApplication.DTOs;
using Domain.Constants.Enums;


namespace Application.UserWallet.DTOs
{
    public class WalletModel
    {
        public Guid Id { get; set; }

        public double TotalBalance { get; set; }

        public Currency Currency { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid UserId { get; set; }

        public UserModel User { get; set; }

        // public List<DepositTransactionModel>? DepositTransactions { get; set; }

        public List<WalletTransactionModel>? WalletTransactions { get; set; }
    }
}
