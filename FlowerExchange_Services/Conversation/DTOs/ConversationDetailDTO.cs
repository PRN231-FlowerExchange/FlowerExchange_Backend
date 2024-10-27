using Application.Message.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.DTOs
{
    public class ConversationDetailDTO
    {
        public Guid Id { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //public Guid createById { get; set; }
        //public Guid updateById { get; set; }
        //public List<MessageConversationDTO>? Messages { get; set; }
        public List<UserConversationDetailDTO> UserConversations { get; set; }
    }
}
