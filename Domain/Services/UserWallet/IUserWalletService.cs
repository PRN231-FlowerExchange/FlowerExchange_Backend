using Domain.Entities;

namespace Domain.Services.UserWallet
{
    public interface IUserWalletService
    {
        public Task<Domain.Entities.Wallet> CreateUserWallet(User user);
    }
}
