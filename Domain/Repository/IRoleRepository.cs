using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Microsoft.AspNet.Identity;


namespace Domain.Repository
{
    public interface IRoleRepository : IRepositoryBase<Role, Guid>
    {

    }
}
