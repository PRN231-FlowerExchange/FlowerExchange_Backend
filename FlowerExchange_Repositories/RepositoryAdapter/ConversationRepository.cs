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
    public class ConversationRepository : RepositoryBase<Conversation, Guid>, IConversationRepository
    {
        private readonly FlowerExchangeDbContext _context;
        public ConversationRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context;
        }

        public async Task AddConversationAsync(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
        }

        public async Task<Conversation> GetConversationByIdAsync(Guid conversationId)
        {
            return _context.Conversations
            .Include(c => c.Messages)
            .Include(c => c.UserConversations)
            .FirstOrDefault(c => c.Id == conversationId);
        }

        public async Task<List<Conversation>> GetConversationsByUserIdAsync(Guid userId)
        {
            var conversations = _context.Conversations
                .Where(c => c.UserConversations.Any(uc => uc.UserId == userId))
                .OrderByDescending(c => c.UserConversations
                    .Where(uc => uc.UserId == userId)
                    .Max(uc => uc.UpdatedAt))
                .Select(c => new Conversation
                {
                    Id = c.Id,
                    Messages = c.Messages, 
                    UserConversations = c.UserConversations
                        .Where(uc => uc.UserId != userId) 
                        .ToList() 
                })
                .ToList();
            return conversations;
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
