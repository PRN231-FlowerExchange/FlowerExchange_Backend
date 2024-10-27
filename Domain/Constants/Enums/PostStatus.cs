using System.ComponentModel.DataAnnotations;

namespace Domain.Constants.Enums
{
    public enum PostStatus
    {
        [Display(Name = "Available")]
        Available,
        [Display(Name = "Sold Out")]
        SoldOut,
        [Display(Name = "Expired")]
        Expired
    }
}
