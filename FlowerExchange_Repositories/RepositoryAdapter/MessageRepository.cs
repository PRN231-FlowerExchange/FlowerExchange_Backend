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
    public class MessageRepository : RepositoryBase<Message, Guid>, IMessageRepository
    {
        private readonly FlowerExchangeDbContext _context;

        public MessageRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork.Context;
        }

        public async Task AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task<List<Message>> GetMessageThreadAsync(Guid userId)
        {
            // Step 1: Find all conversation IDs where the user is a participant
            var conversationIds = _context.UserConversations
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.ConversationId)
                .ToList();

            // Step 2: Retrieve all messages from those conversations and include Conversation details
            var messages = _context.Messages
                .AsNoTracking()
                .Include(m => m.Conversation)
                .Where(m => conversationIds.Contains(m.ConversationId))
                .OrderBy(m => m.SentAt)
                .ToList();

            foreach (var message in messages)
            {
                var conversationLoaded = message.Conversation != null;
                Console.WriteLine($"Message: {message.Content}, Conversation Loaded: {conversationLoaded}");
            }

            var message1 = _context.Messages
            .Include(m => m.Conversation) // Eagerly load the related Conversation
            .OrderBy(m => m.SentAt)
                .ToList();

            foreach (var message2 in message1)
            {
                var conversationLoaded = message2.Conversation != null;
                Console.WriteLine($"Message: {message2.Content}, Conversation Loaded: {conversationLoaded}");
            }

            return messages;
        }

        public async Task SendMessageAsync(Guid senderId, Guid recipientId, string content)
        {
            var senderUser = await _context.Users.FindAsync(senderId);
            var recipientUser = await _context.Users.FindAsync(recipientId);

            // Kiểm tra xem đã có cuộc hội thoại giữa sender và recipient hay chưa
            var existingConversation = _context.UserConversations
                .Where(uc => (uc.UserId == senderId || uc.UserId == recipientId))
                .GroupBy(uc => uc.ConversationId)
                .Where(g => g.Count() == 2) // Có 2 user trong cuộc hội thoại
                .Select(g => g.Key)
                .FirstOrDefault();

            if (existingConversation == default(Guid))
            {
                var newConversation = new Conversation
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = senderUser,
                    UpdatedBy = senderUser,
                    //Messages = new List<Message>(),
                    //UserConversations = new List<UserConversation>()
                };
                await _context.Conversations.AddAsync(newConversation);
                await _context.SaveChangesAsync();

                // Thêm người gửi và người nhận vào cuộc hội thoại
                var senderConversation = new UserConversation
                {
                    UserId = senderId,
                    ConversationId = newConversation.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = senderUser,
                    UpdatedBy = senderUser,
                };

                var recipientConversation = new UserConversation
                {
                    UserId = recipientId,
                    ConversationId = newConversation.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = recipientUser,
                    UpdatedBy = recipientUser,
                };

                await _context.UserConversations.AddRangeAsync(senderConversation, recipientConversation);
                await _context.SaveChangesAsync();

                // Thiết lập conversationId để gửi tin nhắn mới
                existingConversation = newConversation.Id;
            }

            // Tạo tin nhắn mới và thêm vào cơ sở dữ liệu
            var message = new Message
            {
                Content = content,
                SenderId = senderId,
                ConversationId = existingConversation,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<Message> GetMessageByIdAsync(Guid messageId)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<List<Message>> GetMessagesByConversationIdAsync(Guid conversationId)
        {
            return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
