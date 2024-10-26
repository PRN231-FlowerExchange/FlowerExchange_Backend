namespace Application.UserIdentity.DTOs
{
    public class CurrentUserModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string StatusDisplayName { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public DateTime LastLogin { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? WalletId { get; set; }

        public IList<string> Roles { get; set; }
    }
}
