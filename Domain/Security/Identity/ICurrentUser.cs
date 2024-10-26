namespace Domain.Security.Identity
{
    public interface ICurrentUser
    {
        public bool IsAuthenticated { get; }

        public string UserEmail { get; }

        public Guid UserId { get; }
    }
}
