using Application.Services.JwtTokenService;
using Application.UserIdentity.Commands.Register;
using Application.UserIdentity.Services;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

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
        private readonly ILogger<RegisterCommand> _logger;
        private readonly string TokenLoginProviderName = "JWT Authentication";
        private readonly string RefreshTokenName = "RefreshToken";
        private readonly IJwtTokenProvider _jwtTokenService;
        private readonly TokenFactory _tokenFactory;


        public RefreshTokenCommandHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<RegisterCommand>>();
            _jwtTokenService = serviceProvider.GetRequiredService<IJwtTokenProvider>();
            _tokenFactory = serviceProvider.GetRequiredService<TokenFactory>();
        }
        public async Task<AuthenticatedToken> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var principal = await _jwtTokenService.GetPrincipalFromToken(request.AccessToken);
            if (principal == null)
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
            IList<string> roles = await _userManager.GetRolesAsync(user);
            AuthenticatedToken token = await _tokenFactory.GenerateAuthenticatedSignInSuccess(user, roles);

            await _userManager.SetAuthenticationTokenAsync(user, TokenLoginProviderName, RefreshTokenName, token.RefreshToken);
            return token;

        }
    }
}
