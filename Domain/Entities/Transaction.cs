using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Entities
{
    public class Transaction : BaseEntity<Transaction, Guid>
    {
        public double Amount { get; set; }

        public TransStatus Status { get; set; }

        public TransactionType Type { get; set; }

        public virtual Guid FromWallet { get; set; }

        public virtual Guid ToWallet { get; set; }

        public Guid? FlowerOrderId { get; set; }

        public virtual FlowerOrder? FlowerOrder { get; set; }

        public Guid? ServiceOrderId { get; set; }

        public virtual ServiceOrder? ServiceOrder { get; set; }

        public virtual ICollection<WalletTransaction>? WalletTransactions { get; set; }
    }
}
