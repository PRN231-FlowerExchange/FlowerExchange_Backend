using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants.Enums
{
    public enum TransDirection
    {
        [Display(Name = "Plus")]
        Plus,
        [Display(Name = "Minus")]
        Minus
    }
}
