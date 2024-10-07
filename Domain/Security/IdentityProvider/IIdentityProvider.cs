using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
