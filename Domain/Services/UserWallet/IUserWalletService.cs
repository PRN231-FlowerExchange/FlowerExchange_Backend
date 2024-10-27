using Domain.Entities;

namespace Domain.Services.UserWallet
{
    public interface IUserWalletService
    {
        public Task<Wallet> CreateUserWallet(User user);
    }
}
