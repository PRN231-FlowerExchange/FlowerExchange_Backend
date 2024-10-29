using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter;

public class FlowerOrderRepository : RepositoryBase<FlowerOrder, Guid>, IFlowerOrderRepository
{
    public FlowerOrderRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
    {
    }
}