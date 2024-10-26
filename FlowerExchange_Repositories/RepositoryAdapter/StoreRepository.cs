using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter
{
    public class StoreRepository : RepositoryBase<Store, Guid>, IStoreRepository
    {
        public StoreRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
