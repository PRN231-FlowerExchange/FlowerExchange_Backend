using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Persistence.RepositoryAdapter;

public class FlowerOrderRepository : RepositoryBase<FlowerOrder, Guid>, IFlowerOrderRepository
{
    public FlowerOrderRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<List<FlowerOrder>> GetFlowerOrdersByWalletIdAndTypeAndStatus(Guid walletId, TransactionType type, TransStatus status)
    {
        try
        {
            if (!_dbContext.Transactions.Any() || !_dbContext.FlowerOrders.Any())
            {
                return new List<FlowerOrder>();
            }
            
            return await _dbContext.Transactions
                .Include(t => t.FlowerOrder)
                .ThenInclude(fo => fo.Flower)
                .ThenInclude(f => f.Post)
                .Where(t => t.FromWallet.Equals(walletId) && t.Type == type && t.Status == status)
                .Select(t => t.FlowerOrder)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}