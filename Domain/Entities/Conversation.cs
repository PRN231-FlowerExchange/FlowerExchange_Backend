using Domain.Commons.BaseEntities;


namespace Domain.Entities
{
    public class Conversation : BaseEntity<Conversation, Guid>
    {
        public virtual ICollection<UserConversation>? UserConversations { get; set; } = new List<UserConversation>();

        public virtual ICollection<Message>? Messages { get; set; } = new List<Message>();
    }
}
