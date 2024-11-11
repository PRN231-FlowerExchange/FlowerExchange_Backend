using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Trade")] //giao dich mua
        Trade,
        [Display(Name = "Promotion")] //giao dich ban
        Promotion,
        [Display(Name = "Refund")] //giao dich hoan tien
        Refund,
        [Display(Name = "Deposit")] //giao dich nap tien
        Deposit,
        [Display(Name = "Withdraw")] //giao dich rut tien
        Withdraw,
    }
}
