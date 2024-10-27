using Domain.Commons.BaseRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUserConversationRepository : IRepositoryBase<UserConversation, Guid>
    {
        Task AddUserConversationAsync(UserConversation userConversation);
        Task<List<UserConversation>> GetUserConversationsByUserIdAsync(Guid userId);
        Task SaveAsync();
    }
}
