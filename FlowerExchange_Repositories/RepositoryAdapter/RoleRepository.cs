
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.RepositoryAdapter
{
    public class RoleRepository : RepositoryBase<Role, Guid>, IRoleRepository
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork, RoleManager<Role> roleManager)
            : base(unitOfWork)
        {
            _roleManager = roleManager;

        }
    }
}
