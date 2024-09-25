using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
