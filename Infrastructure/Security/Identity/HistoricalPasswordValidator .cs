using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security.Identity
{
    //Create a new password validator to validate password of identity user against historical passwords
    public class HistoricalPasswordValidator : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string? password)
        {
            if (password.Contains("testhistoricalpasssword"))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "HistoricalPassword",
                    Description = "HistoricalPasswordValidator testing",
                }));
            }

            //TODO: Should replcae to implment you own mechanims to check historical password
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
