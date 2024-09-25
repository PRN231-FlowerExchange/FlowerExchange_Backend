using Application.Order.DTOs;
using Application.Wallet.DTOs;
using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.PaymentTransaction.DTOs
{
    public class TransactionModel
    {
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public TransStatus Status { get; set; }

        public TransactionType Type { get; set; }

        public Guid FromWallet { get; set; }

        public Guid ToWallet { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid? FlowerOrderId { get; set; }

        public FlowerOrderModel? FlowerOrder { get; set; }

        public Guid? ServiceOrderId { get; set; }

        public ServiceOrderModel? ServiceOrder { get; set; }

        public List<WalletTransactionModel>? WalletTransactions { get; set; }
    }
}
