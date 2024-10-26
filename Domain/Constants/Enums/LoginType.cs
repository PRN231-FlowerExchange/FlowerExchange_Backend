using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum LoginType
    {
        [Display(Name = "Google")]
        Google,
        [Display(Name = "Basic")]
        Baisc
    }
}
