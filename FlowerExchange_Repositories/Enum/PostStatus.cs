using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Enum
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
