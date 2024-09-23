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
    public class WalletModel
    {
        public Guid Id { get; set; }

        public double TotalBalance {  get; set; }
        
        public Currency Currency { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid UserId { get; set; }

        public UserModel User { get; set; }

        public List<DepositTransactionModel>? DepositTransactions { get; set; }

        public List<WalletTransactionModel>? WalletTransactions { get; set; }
    }
}
