using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Persistence.RepositoryAdapter.DTOs
{
    public class MessageThreadDTO
    {
        public string Content { get; set; }
        [JsonIgnore]
        public bool IsRead { get; set; }
        [JsonIgnore]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public Guid SenderId { get; set; }
        public Guid ConversationId { get; set; }
        public Domain.Entities.Conversation Conversation { get; set; }
        public Guid RecipientId { get; set; }
    }
}
