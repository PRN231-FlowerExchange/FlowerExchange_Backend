using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Services.BusinessModels
{
    public class UserConversationModel
    {
        public Guid UserId { get; set; }

        public UserModel User { get; set; }

        public Guid ConversationId { get; set; }

        public ConversationModel Conversation { get; set; }


    }
}
