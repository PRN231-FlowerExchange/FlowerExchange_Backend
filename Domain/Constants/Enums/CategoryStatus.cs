using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum CategoryStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive
    }
}
