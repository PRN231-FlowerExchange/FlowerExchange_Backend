using Application.PaymentTransaction.DTOs;
using Application.PostFlower.DTOs;
using Application.UserApplication.DTOs;
using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.DTOs
{
    public class FlowerOrderModel
    {
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public bool IsRefund { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid BuyerId { get; set; }

        public UserModel Buyer { get; set; }

        public Guid SellerId { get; set; }

        public UserModel Seller { get; set; }

        public Guid FlowerId { get; set; }

        public FlowerModel Flower { get; set; }

        public ICollection<TransactionModel>? Transactions { get; set; }


    }
}
