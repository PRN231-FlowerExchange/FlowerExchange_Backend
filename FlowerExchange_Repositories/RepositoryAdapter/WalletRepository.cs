using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Wallet?> GetByUserId(Guid userId)
        {
            try
            {
                if (!_dbContext.Wallets.Any())
                {
                    return null;
                }
                return await _dbContext.Users
                    .Where(u => u.Id.Equals(userId))
                    .Select(u => u.Wallet)
                    .FirstOrDefaultAsync();

                //var user = await _context.Users.Include(u => u.Wallet).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
