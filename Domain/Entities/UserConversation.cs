using Domain.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class UserConversation : BaseEntity<UserConversation, Guid>
    {
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public Guid ConversationId { get; set; }

        public virtual Conversation Conversation { get; set; }


    }
}
