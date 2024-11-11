
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.RepositoryAdapter
{
    public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
    {
        private readonly DbSet<User> _dbSetUser;

        private readonly UserManager<User> _userManager;


        public UserRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork, UserManager<User> userManager)
            : base(unitOfWork)
        {
            _userManager = userManager;
            _dbSetUser = base.DbSet;

        }
        public User GetUserByEmail(string email)
        {
            return _dbSetUser.FirstOrDefault(u => u.Email == email);

        }

        public async Task<User> GetUserByWalletId(Guid walletId)
        {
            var users = await _dbContext.Wallets
                .Include(w => w.User)
                .Where(w => w.Id.Equals(walletId))
                .Select(w => w.User)
                .ToListAsync();

            return users.First();
        }
    }


}
