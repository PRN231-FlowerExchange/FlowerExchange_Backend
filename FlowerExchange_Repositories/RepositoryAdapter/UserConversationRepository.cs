using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.RepositoryAdapter
{
    public class UserConversationRepository : RepositoryBase<UserConversation, Guid>, IUserConversationRepository
    {
        private readonly FlowerExchangeDbContext _context;
        public UserConversationRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context;
        }
        public async Task AddUserConversationAsync(UserConversation userConversation)
        {
            await _context.UserConversations.AddAsync(userConversation);
        }

        public async Task<List<UserConversation>> GetUserConversationsByUserIdAsync(Guid userId)
        {
            return await _context.UserConversations
                .Where(uc => uc.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
