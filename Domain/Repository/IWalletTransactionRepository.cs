using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Models;

namespace Domain.Repository
{
    public interface IWalletTransactionRepository : IRepositoryBase<WalletTransaction, Guid>
    {
        Task<PagedList<Transaction>> GetAllWalletTransactionAsync(WalletTransactionParameter walletTransactionParameter);
        
        Task<PagedList<WalletTransaction>> GetWalletTransactionsByWalletIdAsync(Guid walletId, WalletTransactionParameter walletTransactionParameter);
        
        Task<WalletTransaction?> GetWalletTransactionByTransactionIdAsync(Guid transactionId, Guid userId);
    }
}
