using Domain.Commons.BaseRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IWalletRepository : IRepositoryBase<Wallet, Guid>
    {
        Task<Wallet?> GetByUserId(Guid userId);
    }
}
