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
            // Ensure UserConversations is a DbSet in your DbContext
            var conversations = _context.UserConversations
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Conversation)
                .Include(c => c.Messages) // Include all messages
                .ToList(); // Use ToListAsync for asynchronous operation

            // Check if any messages are present
            foreach (var conversation in conversations)
            {
                // If Messages is null, initialize it to an empty list
                conversation.Messages ??= new List<Message>();

                // Log for debugging
                Console.WriteLine($"Conversation ID: {conversation.Id}, Messages Count: {conversation.Messages.Count}");
            }

            var conversations1 = _context.Conversations
                .Include(c => c.Messages)
                .ToList();

            foreach (var conversation in conversations1)
            {
                Console.WriteLine($"Conversation ID: {conversation.Id}, Messages Count: {conversation.Messages.Count}");
            }


            return conversations;
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
