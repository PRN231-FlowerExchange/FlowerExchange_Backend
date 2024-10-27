using Domain.Commons.BaseEntities;

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
