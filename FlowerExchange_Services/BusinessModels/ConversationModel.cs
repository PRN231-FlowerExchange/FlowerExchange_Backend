using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Services.BusinessModels
{
    public class ConversationModel
    {
        public Guid Id { get; set; }

        public DateTime CreateAt { get; set; }

        public List<UserConversationModel>? UserConversations { get; set; }

        public List<MessageModel>? Messages { get; set; }


    }
}
