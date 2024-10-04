using Domain.Commons.BaseRepositories;
using Domain.Entities;


namespace Domain.Repository
{
    public interface IUserRepository : IRepositoryBase<User, Guid>
    {

    }
}
