using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using Domain.Services.UserWallet;
using Microsoft.Extensions.DependencyInjection;
using Persistence;


namespace Application.UserWallet.Services
{
    public class UserWalletService : IUserWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserWalletService(IServiceProvider serviceProvider)
        {
            _walletRepository = serviceProvider.GetRequiredService<IWalletRepository>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        }

        public async Task<Wallet> CreateUserWallet(User user)
        {
            if (user == null || user.Id == null)
            {
                throw new ArgumentNullException("User cannot be null");
            }
            User userdb = await _userRepository.GetByIdAsync(user.Id);
            if (userdb == null)
            {
                throw new ArgumentNullException("User cannot be null");
            }
            Wallet wallet = new Wallet()
            {
                UserId = user.Id,
                TotalBalance = 0,
                Currency = Domain.Constants.Enums.Currency.VND,
            };
            try
            {
                await _walletRepository.InsertAsync(wallet);
                await _unitOfWork.SaveChangesAsync();
                var savedWallet = await _walletRepository.FindByConditionAsync(x => x.UserId == user.Id);
                return savedWallet;
            }
            catch (Exception e)
            {
                _unitOfWork.RollbackChanges();
                throw;
            }



        }
    }
}
