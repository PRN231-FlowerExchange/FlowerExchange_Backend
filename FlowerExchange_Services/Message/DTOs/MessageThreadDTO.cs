using Application.Conversation.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Message.DTOs
{
    public class MessageThreadDTO
    {
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public Guid SenderId { get; set; }
        //public Guid ConversationId { get; set; }
        public ConversationDTO? Conversation { get; set; }
        //[JsonIgnore]
        //public Guid RecipientId { get; set; }
    }
}
