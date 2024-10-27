using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Buy")] //giao dich mua
        Buy,
        [Display(Name = "Sell")] //giao dich ban
        Sell,
        [Display(Name = "Refund")] //giao dich hoan tien
        Refund,
        [Display(Name = "Deposit")] //giao dich nap tien
        Deposit,
        [Display(Name = "Withdraw")] //giao dich rut tien
        Withdraw,
    }
}
