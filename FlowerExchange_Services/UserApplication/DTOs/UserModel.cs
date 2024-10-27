using Application.Conversation.DTOs;
using Application.Order.DTOs;
using Application.PostFlower.DTOs;
using Application.Report.DTOs;
using Application.UserWallet.DTOs;
using Domain.Constants.Enums;


namespace Application.UserApplication.DTOs
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }

        public string RefreshToken { get; set; }

        public LoginType LoginType { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePictureUrl { get; set; }

        public UserStatus Status { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public DateTime LastLogin { get; set; }

        public RoleType Role { get; set; }

        public List<PostModel>? Posts { get; set; }

        public WalletModel? Wallet { get; set; }

        // public List<UserConversationModel>? UserConversations { get; set; }

        public List<MessageModel>? Messages { get; set; }

        public List<ServiceOrderModel>? ServiceOrders { get; set; }

        public List<FlowerOrderModel>? BuyOrders { get; set; }

        public List<FlowerOrderModel>? SellOrders { get; set; }

        public List<ReportModel>? CreatedReports { get; set; }

        public List<ReportModel>? UpdatedReports { get; set; }

    }
}
