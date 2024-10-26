using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter
{
    public class FlowerRepository : RepositoryBase<Flower, Guid>, IFlowerRepository
    {
        public FlowerRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
