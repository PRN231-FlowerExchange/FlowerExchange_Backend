using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter
{
    public class ServiceRepository : RepositoryBase<Service, Guid>, IServiceRepository
    {
        public ServiceRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
