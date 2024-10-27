using Domain.Security.Identity;

namespace Infrastructure.Security.Identity
{
    public class AnoymousUser : ICurrentUser
    {
        public bool IsAuthenticated => false;

        public Guid UserId => Guid.Empty;

        public string UserEmail => null;
    }
}
