using Application.Services.JwtTokenService;
using Application.UserIdentity.Commands.Register;
using Domain.Commons.BaseRepositories;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Application.UserIdentity.Commands.Login
{
    public record class UsernamePasswordLoginCommand : IRequest<AuthenticatedToken>
    {
        [EmailAddress(ErrorMessage = "Invalid Email !")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; init; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }

    }

    public class UsernamePasswordLoginCommandHandler : IRequestHandler<UsernamePasswordLoginCommand, AuthenticatedToken>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<RegisterCommand> _logger;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitofwork;
        private readonly RoleManager<Role> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;
       
        private readonly IJwtTokenProvider _jwtTokenService;


        public UsernamePasswordLoginCommandHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _userStore = serviceProvider.GetRequiredService<IUserStore<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<RegisterCommand>>();
            _unitofwork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
            _jwtTokenService = serviceProvider.GetRequiredService<IJwtTokenProvider>();
        }
        public async Task<AuthenticatedToken> Handle(UsernamePasswordLoginCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var signInResult = await _signInManager.PasswordSignInAsync(
                userName: request.Email,
                password: request.Password,
                isPersistent: false,
                lockoutOnFailure: true);

            if (signInResult.Succeeded)
            {
                User user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null)
                {
                    throw new NotFoundException(request.Email, nameof(User));
                }

                AuthenticatedToken token =  await this.GenerateAuthenticatedSignInSuccess(user);
                
                await _userManager.SetAuthenticationTokenAsync(user, TokenConstants.TOKEN_LOGIN_PROVIDER_NAME, TokenConstants.REFRESH_TOKEN_NAME, token.RefreshToken);
                return token;
            }
            else if (signInResult.RequiresTwoFactor)
            {
                throw new BadHttpRequestException("Sign In required two factor verify");
            }
            else if (signInResult.IsLockedOut)
            {
                throw new BadHttpRequestException("Your account has been locked ! ");
            }
            else if (signInResult.IsNotAllowed)
            {
                throw new UnauthorizedAccessException("Access Denied");
            }
            else
            {
                throw new BadHttpRequestException("Login failed ! Username or password is incorrect!");
            }
            
        }

        private async Task<AuthenticatedToken> GenerateAuthenticatedSignInSuccess(User user)
        {
            IList<String> roles = await _userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>() {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                  new Claim(ClaimTypes.Name, user.Email),
                  new Claim(ClaimTypes.GivenName, user.Fullname),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));



            var accessToken = _jwtTokenService.GenerateAccessToken(claims, TokenConstants.ACCESS_TOKEN_PERIOD_MINISECOND);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(TokenConstants.REFRESH_TOKEN_PERIOD_MINISECOND);
            AuthenticatedToken authenticatedToken = new AuthenticatedToken()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer"
            };
            return authenticatedToken;
        }
    }
}
