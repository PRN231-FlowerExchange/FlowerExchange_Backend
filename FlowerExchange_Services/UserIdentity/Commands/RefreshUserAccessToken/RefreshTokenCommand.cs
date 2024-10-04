using Application.Services.JwtTokenService;
using Application.UserIdentity.Commands.Register;
using Domain.Commons.BaseRepositories;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Security.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Application.UserIdentity.Commands.RefreshUserAccessToken
{
    public class RefreshTokenCommand : IRequest<AuthenticatedToken>
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
       
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticatedToken>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<RegisterCommand> _logger;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitofwork;
        private readonly RoleManager<Role> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;
        private readonly string TokenLoginProviderName = "JWT Authentication";
        private readonly string RefreshTokenName = "RefreshToken";
        private readonly IJwtTokenProvider _jwtTokenService;
        private readonly ICurrentUser _currentuser;


        public RefreshTokenCommandHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _userStore = serviceProvider.GetRequiredService<IUserStore<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<RegisterCommand>>();
            _unitofwork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
            _jwtTokenService = serviceProvider.GetRequiredService<IJwtTokenProvider>();
            _currentuser = serviceProvider.GetRequiredService<ICurrentUser>();
        }
        public async Task<AuthenticatedToken> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var principal = await _jwtTokenService.GetPrincipalFromToken(request.AccessToken);
            if(principal == null)
            {
                throw new BadRequestException("Token is invalid");
            }
            var user = await _userManager.FindByEmailAsync(principal.Identity.Name);
            if (user == null)
            {
                throw new NotFoundException("User not found !");
            }
            string storedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, TokenConstants.TOKEN_LOGIN_PROVIDER_NAME, TokenConstants.REFRESH_TOKEN_NAME);
            if (storedRefreshToken == null || storedRefreshToken != request.RefreshToken)
            {
                throw new BadRequestException("Invalid refresh token !");
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var readableToken = tokenHandler.ReadJwtToken(storedRefreshToken);
            Console.WriteLine("==== CHECK REFRESH TOKEN EXPIRED");
            Console.WriteLine("==== readableToken.ValidTo " + readableToken.ValidTo);
            Console.WriteLine("==== datetime now " + DateTime.UtcNow);
            if (readableToken.ValidTo.CompareTo(DateTime.UtcNow) <= 0)
            {
                Console.WriteLine("TOKEN EXPIRED NEED TO BE REFRESHED");
                throw new BadRequestException($"Refresh token expired at {readableToken.ValidTo}!");
            }

            AuthenticatedToken token = await this.GenerateAuthenticatedSignInSuccess(user);

            await _userManager.SetAuthenticationTokenAsync(user, TokenLoginProviderName, RefreshTokenName, token.RefreshToken);
            return token;

        }

        private async Task<AuthenticatedToken> GenerateAuthenticatedSignInSuccess(User user)
        {
            List<Claim> claims = new List<Claim>() {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(ClaimTypes.Name, user.Email)
            };

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
