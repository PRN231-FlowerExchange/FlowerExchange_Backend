using Application.Services.JwtTokenService;
using Domain.Constants;
using Domain.Entities;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.UserIdentity.Services
{
    public class TokenFactory
    {
        private readonly IJwtTokenProvider _jwtTokenService;

        public TokenFactory(IServiceProvider serviceProvider)
        {
            _jwtTokenService = serviceProvider.GetRequiredService<IJwtTokenProvider>();
        }

        public async Task<AuthenticatedToken> GenerateAuthenticatedSignInSuccess(User user, IList<string> roles)
        {
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
