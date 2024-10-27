using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Conversation.DTOs
{
    public class UserConversationDetailDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserMessageDTO User { get; set; }
    }
}
