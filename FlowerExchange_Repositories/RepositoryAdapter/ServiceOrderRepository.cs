using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter
{
    public class ServiceOrderRepository : RepositoryBase<ServiceOrder, Guid>, IServiceOrderRepository
    {

        public ServiceOrderRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
