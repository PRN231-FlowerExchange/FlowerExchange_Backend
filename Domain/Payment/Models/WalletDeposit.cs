﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payment.Models
{
    public class WalletDeposit
    {
        public double Amount { get; set; }

        public string UserId { get; set; }
    }
}
