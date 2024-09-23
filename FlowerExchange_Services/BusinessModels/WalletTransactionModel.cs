using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Services.BusinessModels
{
    public class WalletTransactionModel
    {
        public Guid WalletId { get; set; }

        public WalletModel Wallet { get; set; }

        public Guid TransactonId { get; set; }

        public TransactionModel Transaction { get; set; }

        public TransDirection Type { get; set; }
    }
}
