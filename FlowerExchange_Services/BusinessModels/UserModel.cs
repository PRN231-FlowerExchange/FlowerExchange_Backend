using FlowerExchange_Repositories.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Services.BusinessModels
{
    public class UserModel
    {
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

        public StoreModel? Store { get; set; }

        public List<PostModel>? Posts { get; set; }

        public WalletModel? Wallet { get; set; }

        public List<UserConversationModel>? UserConversations { get; set;}

        public List<MessageModel>? Messages { get; set; }

        public List<ServiceOrderModel>? ServiceOrders { get; set; }

        public List<FlowerOrderModel>? BuyOrders { get; set; }

        public List<FlowerOrderModel>? SellOrders { get; set; }

        public List<ReportModel>? CreatedReports { get; set; }

        public List<ReportModel>? UpdatedReports { get; set; }

    }
}
