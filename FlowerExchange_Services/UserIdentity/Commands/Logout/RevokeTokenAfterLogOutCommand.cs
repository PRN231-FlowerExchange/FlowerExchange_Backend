using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Security.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;


namespace Application.UserIdentity.Commands.Logout
{

    public record RevokeTokenAfterLogOutCommand : IRequest
    {

    }

    public class RevokeTokenAfterLogOutCommandHandler : IRequestHandler<RevokeTokenAfterLogOutCommand>
    {
        private readonly ICurrentUser _curentUser;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _context;

        public RevokeTokenAfterLogOutCommandHandler(IServiceProvider serviceProvider, IHttpContextAccessor context)
        {
            _curentUser = serviceProvider.GetRequiredService<ICurrentUser>();
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
            this._context = context;

        }

        public async Task Handle(RevokeTokenAfterLogOutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid userId = _curentUser.UserId;
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new NotFoundException("User not found during logout.");
                }

                // Revoke refresh token
                string refreshToken = await _userManager.GetAuthenticationTokenAsync(user, TokenConstants.TOKEN_LOGIN_PROVIDER_NAME, TokenConstants.REFRESH_TOKEN_NAME);
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await _userManager.RemoveAuthenticationTokenAsync(user, TokenConstants.TOKEN_LOGIN_PROVIDER_NAME, TokenConstants.REFRESH_TOKEN_NAME);
                }

                // Sign out the user and invalidate the authentication session
                await _signInManager.SignOutAsync();
                _context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // Clear identity
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}

