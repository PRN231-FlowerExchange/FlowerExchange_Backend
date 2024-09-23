﻿using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Services.BusinessModels
{
    public class ServiceOrderModel
    {
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public OrderStatus Status { get; set; }

        public Guid BuyerId { get; set; }

        public UserModel Buyer {  get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public List<PostServiceModel> PostServices { get; set; }

        public List<TransactionModel>? Transactions { get; set; }
    }
}
