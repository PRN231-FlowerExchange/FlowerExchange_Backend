using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UserStore.DTOs;

namespace Application.Conversation.DTOs
{
    public class MessageModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Guid SenderId { get; set; }

        public UserModel Sender { get; set; }

        public Guid ConversationId { get; set; }

        public ConversationModel Conversation { get; set; }


    }
}
