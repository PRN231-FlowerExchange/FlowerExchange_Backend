using Domain.Entities;

namespace Domain.Security.IdentityProvider
{
    public interface IIdentityProvider
    {
        Task<IList<User>> GetUsersAsync();

        Task<User> GetUserById(string userId);

        Task<User> GetUserByUsernameAsync(string username);

        Task CreateUserAsync(User user);

        Task UpdateUserAsync(string userId, User user);

        Task DeleteUserAsync(string userId);


    }
}
