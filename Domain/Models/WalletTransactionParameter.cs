using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class WalletTransactionParameter : QueryStringParameters
    {
        public string? Title { get; set; } = null;
        public WalletTransactionParameter()
        {
            OrderBy = "CreatedAt";
        }
    }
}
