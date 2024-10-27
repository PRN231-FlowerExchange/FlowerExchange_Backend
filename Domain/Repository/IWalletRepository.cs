using Domain.Commons.BaseRepositories;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IWalletRepository : IRepositoryBase<Wallet, Guid>
    {
        Task<Wallet?> GetByUserId(Guid userId);
    }
}
