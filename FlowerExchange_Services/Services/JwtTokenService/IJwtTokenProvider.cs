using System.Security.Claims;


namespace Application.Services.JwtTokenService
{
    public interface IJwtTokenProvider
    {
        public string GenerateAccessToken(List<Claim> claims, int milisecondExpired);
        public string GenerateRefreshToken(int milisecondExpired);
        public Task<ClaimsPrincipal> ReadAndValidateTokenAsync(string token);
        public Task<ClaimsPrincipal> GetPrincipalFromToken(string token);

    }
}
