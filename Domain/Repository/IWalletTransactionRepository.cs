using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Models;

namespace Domain.Repository
{
    public interface IWalletTransactionRepository : IRepositoryBase<WalletTransaction, Guid>
    {
        Task<PagedList<WalletTransaction>> GetAllWalletTransactionAsync(WalletTransactionParameter walletTransactionParameter);
    }
}
