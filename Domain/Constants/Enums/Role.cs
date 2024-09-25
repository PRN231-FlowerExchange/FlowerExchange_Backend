using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants.Enums
{
    public enum Role
    {
        [Display(Name = "Admin")]
        Admin,
        [Display(Name = "Individual")]
        Individual,
        [Display(Name = "Business")]
        Business
    }
}
