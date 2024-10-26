using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Message.DTOs
{
    public class MessageDTO
    {
        public string Content { get; set; }
        [JsonIgnore]
        public bool IsRead { get; set; }
        [JsonIgnore]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public Guid SenderId { get; set; }
        [JsonIgnore]
        public Guid ConversationId { get; set; }
        public Guid RecipientId { get; set; }
    }
}
