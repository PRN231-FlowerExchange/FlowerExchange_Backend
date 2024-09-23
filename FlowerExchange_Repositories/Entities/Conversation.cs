﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("Conversation")]
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreateAt { get; set; }

        public ICollection<UserConversation>? UserConversations { get; set; }

        public ICollection<Message>? Messages { get; set; }


    }
}
