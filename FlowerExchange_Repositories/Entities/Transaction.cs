using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FlowerExchange_Repositories.Entities
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public TransStatus Status {  get; set; }

        public TransactionType Type { get; set; }

        public Guid FromWallet {  get; set; }

        public Guid ToWallet { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid? FlowerOrderId { get; set; }

        public FlowerOrder? FlowerOrder { get; set; }

        public Guid? ServiceOrderId { get; set; }

        public ServiceOrder? ServiceOrder { get; set; }

        public ICollection<WalletTransaction>? WalletTransactions { get; set; }
    }
}
