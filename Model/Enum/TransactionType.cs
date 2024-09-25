using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Enum
{
    public enum TransactionType
    {
        [Display(Name = "Buy")]
        Buy,
        [Display(Name = "Sell")]
        Sell,
        [Display(Name = "Refund")]
        Refund
    }
}
