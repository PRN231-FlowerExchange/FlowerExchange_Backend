﻿using Application.Order.DTOs;
using Application.UserWallet.DTOs;
using Domain.Constants.Enums;

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
