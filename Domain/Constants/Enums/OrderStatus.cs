using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Success")]
        Success,
        [Display(Name = "Failed")]
        Failed,
        [Display(Name = "Pending Payment")]
        PendingPayment
    }
}
