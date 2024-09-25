using Domain.Commons.BaseEntities;


namespace Domain.Entities
{
    public class Message : BaseEntity<Message, Guid>
    {
        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Guid SenderId { get; set; }

        public virtual User Sender { get; set; }

        public Guid ConversationId { get; set; }

        public virtual Conversation Conversation { get; set; }


    }
}
