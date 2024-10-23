using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.RepositoryAdapter
{
    public class WalletRepository : RepositoryBase<Wallet, Guid>, IWalletRepository
    {
        public WalletRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
