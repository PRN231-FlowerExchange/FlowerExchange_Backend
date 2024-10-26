using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum PostServiceStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive
    }
}
