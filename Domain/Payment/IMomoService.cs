using Domain.Payment.Models;

namespace Domain.Payment
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(WalletDeposit model, string currentPath);
    }
}
