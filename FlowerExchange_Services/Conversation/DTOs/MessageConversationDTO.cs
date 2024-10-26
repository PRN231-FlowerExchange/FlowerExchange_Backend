using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Conversation.DTOs
{
    public class MessageConversationDTO
    {
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public Guid SenderId { get; set; }
        public UserMessageDTO Sender { get; set; }
        [JsonIgnore]
        public Guid ConversationId { get; set; }
    }
}
