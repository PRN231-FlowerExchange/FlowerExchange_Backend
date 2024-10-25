using Domain.Commons.BaseEntities;
using Domain.Entities;


namespace Domain.Events.UserEvents
{
    public class UserRegisteredCompleteEvent : BaseEvent
    {
        public User User { get; }

        public UserRegisteredCompleteEvent(User user) 
        {
            this.User = user;
        }

       
    }
}
