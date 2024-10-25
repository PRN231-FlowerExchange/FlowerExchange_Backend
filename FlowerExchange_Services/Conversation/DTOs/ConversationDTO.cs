using Application.Message.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Conversation.DTOs
{
    public class ConversationDTO
    {
        //[JsonIgnore]
        //public virtual ICollection<UserConversation>? UserConversations { get; set; }
        //[JsonIgnore]
        //public List<Guid> UserIds { get; set; } = new List<Guid>();
        //[JsonIgnore]
        //public virtual ICollection<MessageDTO>? Messages { get; set; }

        public Guid Id { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        public Guid createById { get; set; }
        public Guid updateById { get; set; }
        public List<MessageDTO>? Messages { get; set; }
    }
}
