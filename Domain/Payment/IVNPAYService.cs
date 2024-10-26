using Domain.Payment.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.Payment
{
    public interface IVNPAYService
    {
        string CreatePaymentUrl(WalletDeposit walletDeposit, HttpContext context, string currentPath);

        VNPAYPaymentResponseModel? PaymentExecute(IQueryCollection collections);
    }
}
