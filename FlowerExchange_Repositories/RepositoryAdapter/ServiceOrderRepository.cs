using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter
{
    public class ServiceOrderRepository : RepositoryBase<ServiceOrder, Guid>, IServiceOrderRepository
    {
        private readonly FlowerExchangeDbContext _context;

        public ServiceOrderRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context;
        }
    }
}
