using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("DepositTransaction")]
    public class DepositTransaction
    {
        [Key]
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public DateTime CreateAt { get; set; }

        public Guid WalletId { get; set; }

        public Wallet Wallet { get; set; }
    }
}
