using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using Domain.Security.IdentityProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class User : BaseEntity<User, Guid>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }

        public string RefreshToken { get; set; }

        public LoginType LoginType { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePictureUrl { get; set; }

        public UserStatus Status { get; set; }

        public DateTime LastLogin { get; set; }

        public Role Role { get; set; }

        public virtual Store? Store { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }

        public virtual Wallet? Wallet { get; set; }

        public virtual ICollection<UserConversation>? UserConversations { get; set; }

        public virtual ICollection<Message>? Messages { get; set; }

        public virtual ICollection<ServiceOrder>? ServiceOrders { get; set; }

        public virtual ICollection<FlowerOrder>? BuyOrders { get; set; }

        public virtual ICollection<FlowerOrder>? SellOrders { get; set; }

        public virtual ICollection<Report>? Reports { get; set; }

    }
}
