using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("Message")]
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Guid SenderId { get; set; }

        public User Sender { get; set; }

        public Guid ConversationId { get; set; }

        public Conversation Conversation { get; set; }


    }
}
