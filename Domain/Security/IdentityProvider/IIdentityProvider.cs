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
        Task<IList<IUserAccount>> GetUsersAsync();

        Task<IUserAccount> GetUserById(string userId);

        Task<IUserAccount> GetUserByUsernameAsync(string username);

        Task CreateUserAsync(IUserAccount user);

        Task UpdateUserAsync(string userId, IUserAccount user);

        Task DeleteUserAsync(string userId);
    }
}
