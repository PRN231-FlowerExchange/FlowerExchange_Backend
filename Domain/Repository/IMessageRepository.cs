using Domain.Commons.BaseRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IMessageRepository : IRepositoryBase<Message, Guid>
    {
        Task<List<Message>> GetMessagesByConversationIdAsync(Guid conversationId);
        Task<Message> GetMessageByIdAsync(Guid messageId);
        Task AddMessageAsync(Message message);
        Task SaveAsync();
        Task SendMessageAsync(Guid senderId, Guid recipientId, string content);
        Task<List<Message>> GetMessageThreadAsync(Guid userId);
    }
}
