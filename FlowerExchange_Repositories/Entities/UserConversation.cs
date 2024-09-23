using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("UserConversation")]
    public class UserConversation
    {
        [Key]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Key]
        public Guid ConversationId { get; set; }

        public Conversation Conversation { get; set; }


    }
}
