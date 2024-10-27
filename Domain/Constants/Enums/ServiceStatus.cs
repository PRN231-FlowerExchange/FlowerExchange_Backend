using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum ServiceStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive,
        [Display(Name = "Suspended")]
        Suspended
    }
}
