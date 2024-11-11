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

        public async Task<List<Message>> GetMessageThreadAsync(Guid conversationId)
        {
            var messages = _context.Messages
                   .Where(m => m.ConversationId == conversationId) 
                   .Include(m => m.Conversation)
                   .Include(m => m.Sender)
                   .AsNoTracking()
                   .OrderBy(m => m.SentAt) 
                   .ToList();

            return messages;
        }

        public async Task SendMessageAsync(Guid senderId, Guid recipientId, string content)
        {
            var senderUser = await _context.Users.FindAsync(senderId);
            var recipientUser = await _context.Users.FindAsync(recipientId);

            // Kiểm tra xem đã có cuộc hội thoại giữa sender và recipient hay chưa
            try
            {
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
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = senderUser,
                        UpdatedBy = senderUser,
                    };
                    await _context.Conversations.AddAsync(newConversation);
                    await _context.SaveChangesAsync();

                    // Thêm người gửi và người nhận vào cuộc hội thoại
                    var senderConversation = new UserConversation
                    {
                        UserId = senderId,
                        ConversationId = newConversation.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = senderUser,
                        UpdatedBy = senderUser,
                    };

                    var recipientConversation = new UserConversation
                    {
                        UserId = recipientId,
                        ConversationId = newConversation.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = recipientUser,
                        UpdatedBy = recipientUser,
                    };

                    await _context.UserConversations.AddRangeAsync(senderConversation, recipientConversation);
                    await _context.SaveChangesAsync();

                    // Thiết lập conversationId để gửi tin nhắn mới
                    existingConversation = newConversation.Id;
                }
                var message = new Message
                {
                    Content = content,
                    SenderId = senderId,
                    ConversationId = existingConversation,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                await _context.Messages.AddAsync(message);

                var conversation = await _context.Conversations.FindAsync(existingConversation);
                if (conversation != null)
                {
                    conversation.UpdatedAt = DateTime.UtcNow;
                    _context.Conversations.Update(conversation);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw;
            }
            // Tạo tin nhắn mới và thêm vào cơ sở dữ liệu
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
