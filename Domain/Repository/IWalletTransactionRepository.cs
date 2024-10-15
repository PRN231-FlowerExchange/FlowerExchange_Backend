using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IWalletTransactionRepository : IRepositoryBase<WalletTransaction, Guid>
    {
        Task<PagedList<WalletTransaction>> GetAllWalletTransactionAsync(WalletTransactionParameter walletTransactionParameter);
    }
}
