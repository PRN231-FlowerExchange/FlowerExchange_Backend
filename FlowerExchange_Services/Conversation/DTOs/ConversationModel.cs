namespace Application.Conversation.DTOs
{
    public class ConversationModel
    {
        public Guid Id { get; set; }

        public DateTime CreateAt { get; set; }

        //        public List<UserConversationModel>? UserConversations { get; set; }

        public List<MessageModel>? Messages { get; set; }


    }
}
