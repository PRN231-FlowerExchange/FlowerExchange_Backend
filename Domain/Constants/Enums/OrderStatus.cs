﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Success")]
        Success,
        [Display(Name = "Failed")]
        Failed,
        [Display(Name = "Pending Payment")]
        PendingPayment
    }
}
