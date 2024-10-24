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
        private readonly FlowerExchangeDbContext _context;

        public WalletRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context;
        }

        public async Task<Wallet?> GetByUserId(Guid userId)
        {
            try
            {
                if (!_context.Wallets.Any())
                {
                    return null;
                }
                return await _context.Users
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
