using Application.UserIdentity.Services;
using Domain.Commons.BaseRepositories;
using Domain.Constants;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Events.UserEvents;
using Domain.Exceptions;
using Domain.Models;
using Duende.IdentityServer.Extensions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;
using Persistence;
using System.Security.Claims;

namespace Application.UserIdentity.Commands.ExternalLogin
{
    public class CallbackExternalLoginCommand : IRequest<AuthenticatedToken>
    {
    }

    public class CallbackExternalLoginCommandHandler : IRequestHandler<CallbackExternalLoginCommand, AuthenticatedToken>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<CallbackExternalLoginCommandHandler> _logger;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitofwork;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenFactory _tokenFactory;
        private readonly IPublisher _publisher; // or IMediator _mediator


        public CallbackExternalLoginCommandHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _userStore = serviceProvider.GetRequiredService<IUserStore<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<CallbackExternalLoginCommandHandler>>();
            _unitofwork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            _signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
            _tokenFactory = serviceProvider.GetRequiredService<TokenFactory>();
            _publisher = serviceProvider.GetRequiredService<IPublisher>(); // Inject IPublisher

        }

        public async Task<AuthenticatedToken> Handle(CallbackExternalLoginCommand request, CancellationToken cancellationToken)
        {
            ExternalLoginInfo externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            Console.WriteLine(externalLoginInfo);
            if (externalLoginInfo == null)
            {
                throw new ArgumentNullException("External Login Info is null");
            }

            var accessToken = externalLoginInfo.AuthenticationTokens.FirstOrDefault(x => x.Name.Equals("access_token"));

            var info = new UserLoginInfo(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, externalLoginInfo.ProviderDisplayName);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user != null)
            {
                if (!await _signInManager.CanSignInAsync(user))
                {
                    throw new ApplicationException("User is not allowed to sign in !");
                }
                if (await _userManager.IsLockedOutAsync(user))
                {
                    throw new ApplicationException("User is locked out !");
                }
                // Sign in the user if they are allowed
                await _signInManager.SignInAsync(user, isPersistent: false);

                IList<string> roles = await _userManager.GetRolesAsync(user);
                AuthenticatedToken token = await _tokenFactory.GenerateAuthenticatedSignInSuccess(user, roles);
                await _userManager.SetAuthenticationTokenAsync(user, TokenConstants.TOKEN_LOGIN_PROVIDER_NAME, TokenConstants.REFRESH_TOKEN_NAME, token.RefreshToken);

                return token;
            }

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var userName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var userFullName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Name);
            var userPhone = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.HomePhone);
            var profile = externalLoginInfo.Principal.FindFirstValue("picture");

            user = await _userManager.FindByEmailAsync(email); //check if user email is registered before by another login provider 
            if (user != null)
            {
                IdentityResult resultLogin = await _userManager.AddLoginAsync(user, info);
                if (!resultLogin.Succeeded)
                {
                    List<ValidationFailure> errors = resultLogin.Errors.Select(e => new ValidationFailure(e.Code, e.Description)).ToList();
                    throw new ValidationException(errors);
                }
                // Sign in the user if the external login was added successfully
                await _signInManager.SignInAsync(user, isPersistent: false);

                IList<string> roles = await _userManager.GetRolesAsync(user);
                AuthenticatedToken token = await _tokenFactory.GenerateAuthenticatedSignInSuccess(user, roles);
                await _userManager.SetAuthenticationTokenAsync(user, TokenConstants.TOKEN_LOGIN_PROVIDER_NAME, TokenConstants.REFRESH_TOKEN_NAME, token.RefreshToken);

                return token;
            }

            user = new User
            {
                Email = email,
                UserName = userName,
                PhoneNumber = userPhone,
                LockoutEnabled = true,
                EmailConfirmed = true,
                ProfilePictureUrl = profile,
                Fullname = userFullName,
                Status = UserStatus.Active,
                LastLogin = DateTime.UtcNow,

            };
            // Start a transaction using DbContext
            using var transaction = await _unitofwork.Context.Database.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("Starting transaction for user login by external login.");

                // Check if role exists
                var roleExists = await _roleManager.RoleExistsAsync(RoleType.Customer.GetDisplayName());

                if (!roleExists)
                {
                    var role = new Role { RoleType = RoleType.Customer, Name = RoleType.Customer.GetDisplayName() };
                    var roleResult = await _roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception("Failed to create role.");
                    }
                }

                // Set username for the user
                await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);

                // Create user in UserManager
                IdentityResult resultCreate = await _userManager.CreateAsync(user);

                if (!resultCreate.Succeeded)
                {
                    List<ValidationFailure> errors = resultCreate.Errors.Select(e => new ValidationFailure(e.Code, e.Description)).ToList();
                    throw new ValidationException(errors);
                }

                // Retrieve saved user
                var userSaved = await _userManager.FindByEmailAsync(user.Email);

                // Add the user to the role
                await _userManager.AddToRoleAsync(user, RoleType.Customer.GetDisplayName());

                // Commit the transaction if everything succeeded
                await _unitofwork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
                
                await _publisher.Publish(new UserRegisteredCompleteEvent(userSaved), cancellationToken);
                
                await _signInManager.SignInAsync(user, isPersistent: false);

                IList<string> roles = await _userManager.GetRolesAsync(user);
                AuthenticatedToken token = await _tokenFactory.GenerateAuthenticatedSignInSuccess(user, roles);
                await _userManager.SetAuthenticationTokenAsync(user, TokenConstants.TOKEN_LOGIN_PROVIDER_NAME, TokenConstants.REFRESH_TOKEN_NAME, token.RefreshToken);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user. Rolling back transaction.");

                // Rollback transaction if any error occurs
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
