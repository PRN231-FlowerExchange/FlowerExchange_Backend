using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Infrastructure.Security.Identity
{
    public class WeakPasswordValidator : IPasswordValidator<User>
    {
        public const int MinimumLength = 8;

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "EmptyPassword",
                    Description = "Password cannot be empty.",
                }));
            }


            if (string.Equals(user.UserName, password, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameAsPassword",
                    Description = "You cannot use your username as your password"
                }));
            }

            if (password.Length < MinimumLength)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "ShortPassword",
                    Description = $"Password must be at least {MinimumLength} characters long.",
                }));
            }


            if (!Regex.IsMatch(password, @"[0-9]") || !Regex.IsMatch(password, @"[\W_]") || !Regex.IsMatch(password, @"[A-Z]"))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "SimplePassword",
                    Description = "Password must contain at least one digit, one special character, one uppercase letter",
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }


    }
}