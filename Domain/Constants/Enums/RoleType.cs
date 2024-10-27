using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum RoleType
    {
        [Display(Name = "Admin")]
        Admin,
        [Display(Name = "Customer")]
        Customer,
    }
}
