using Domain.Payment.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payment
{
    public interface IVNPAYService
    {
        string CreatePaymentUrl(WalletDeposit walletDeposit, HttpContext context, string currentPath);

        VNPAYPaymentResponseModel? PaymentExecute(IQueryCollection collections);
    }
}
