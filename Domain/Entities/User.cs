using Domain.Commons.BaseEntities;
using Domain.Constants.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{

    public class User : IdentityUser<Guid>, IEntityWitkKey<Guid>
    {
        //Domain Key
        [Key]
        public override Guid Id { get; set; } = default!;

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public Guid? createById { get; set; }

        public Guid? updateById { get; set; }

        [NotMapped]
        public User? CreatedBy { get; set; }

        [NotMapped]
        public User? UpdatedBy { get; set; }

        public string? Fullname { get; set; }

        public string? RefreshToken { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public UserStatus? Status { get; set; } = UserStatus.Active;

        public DateTime? LastLogin { get; set; }

        public virtual Store? Store { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }

        public virtual Wallet? Wallet { get; set; }

        public virtual ICollection<UserConversation>? UserConversations { get; set; }

        public virtual ICollection<Message>? Messages { get; set; }

        public virtual ICollection<ServiceOrder>? ServiceOrders { get; set; }

        public virtual ICollection<FlowerOrder>? BuyOrders { get; set; }

        public virtual ICollection<FlowerOrder>? SellOrders { get; set; }

        public virtual ICollection<Report>? Reports { get; set; }

        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }

        public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }

        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set;}


    }
}
