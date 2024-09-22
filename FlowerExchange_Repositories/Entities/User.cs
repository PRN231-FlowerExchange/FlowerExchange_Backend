using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }

        public string RefreshToken { get; set; }

        public LoginType LoginType { get; set; }

        public string Email {  get; set; }

        public string PhoneNumber {  get; set; }

        public string ProfilePictureUrl { get; set; }

        public UserStatus Status { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public DateTime LastLogin {  get; set; }

        public Role Role { get; set; }

        public Store? Store { get; set; }

        public ICollection<Post>? Posts { get; set; }

        public Wallet? Wallet { get; set; }

    }
}
