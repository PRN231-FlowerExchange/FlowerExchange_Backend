using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum TransStatus
    {
        [Display(Name = "Success")]
        Success,
        [Display(Name = "Fail")]
        Fail
    }
}
