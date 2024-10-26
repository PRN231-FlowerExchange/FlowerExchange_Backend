using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum UserStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive,
        [Display(Name = "Banned")]
        Banned
    }
}
