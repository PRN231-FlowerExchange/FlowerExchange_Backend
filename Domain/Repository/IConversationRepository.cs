using Domain.Commons.BaseRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IConversationRepository : IRepositoryBase<Conversation, Guid>
    {
        Task<Conversation> GetConversationByIdAsync(Guid conversationId);
        Task<List<Conversation>> GetConversationsByUserIdAsync(Guid userId);
        Task AddConversationAsync(Conversation conversation);
        Task SaveAsync();
    }
}
