using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("WalletTransaction")]
    public class WalletTransaction
    {
        [Key]
        public Guid WalletId { get; set; }

        public Wallet Wallet { get; set; }

        [Key]
        public Guid TransactonId { get; set; }

        public Transaction Transaction { get; set; }

        public TransDirection Type { get; set; }
    }
}
