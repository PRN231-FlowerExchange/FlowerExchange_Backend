using Domain.Entities;

namespace Domain.Security.Identity
{
    public interface IPasswordHasher
    {
        bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
    }
}
