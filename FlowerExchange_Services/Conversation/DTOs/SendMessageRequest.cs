using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.DTOs
{
    public class SendMessageRequest
    {
        public Guid SenderId { get; set; }
        public Guid? ConversationId { get; set; } // Có thể null nếu chưa có cuộc hội thoại
        public string Content { get; set; }
        public Guid ReceiverId { get; set; } // Người nhận
    }
}
