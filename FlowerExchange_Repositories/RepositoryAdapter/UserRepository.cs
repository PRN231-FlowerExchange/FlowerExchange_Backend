
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Persistence.RepositoryAdapter
{
    public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
    {
        private readonly DbSet<User> _dbSetUser;

        public UserRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _dbSetUser = base.DbSet;
        }

        public User GetUserByEmail(string email)
        {
            return _dbSetUser.FirstOrDefault(u => u.Email == email);
        }
    }
}
