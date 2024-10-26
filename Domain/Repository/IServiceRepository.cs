using Domain.Commons.BaseRepositories;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IServiceRepository : IRepositoryBase<Service, Guid>
    {
    }
}
