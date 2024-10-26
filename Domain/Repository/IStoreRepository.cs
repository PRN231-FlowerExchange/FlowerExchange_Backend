using Domain.Commons.BaseRepositories;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IStoreRepository : IRepositoryBase<Store, Guid>
    {
    }
}
