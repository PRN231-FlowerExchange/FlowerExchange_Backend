using Domain.Commons.BaseRepositories;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IServiceOrderRepository : IRepositoryBase<ServiceOrder, Guid>
    {
    }
}
