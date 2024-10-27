using Application.Services.JwtTokenService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.Security.TokenProvider
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly ILogger<JwtTokenProvider> _logger;
        private JwtConfigOptions _jwtOptions;

        public JwtTokenProvider(IServiceProvider serviceProvider, IOptions<JwtConfigOptions> jwtOptions)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<JwtTokenProvider>>();
            _jwtOptions = jwtOptions.Value;
            Console.WriteLine("SECRETKY IN TOKEN PROVIDER: " + _jwtOptions.JwtSecret);

        }

        public string GenerateAccessToken(List<Claim> claims, int milisecondExpired)
        {
            var claimsIdentity = new ClaimsIdentity(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMilliseconds(milisecondExpired),
                Issuer = _jwtOptions.JwtValidIssuer,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Audience = _jwtOptions.JwtValidAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.JwtSecret)), SecurityAlgorithms.HmacSha256),
                TokenType = "Access JWT Token"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var JwtToken = tokenHandler.CreateToken(tokenDescriptor); //convert to jwt token format
            var serializeTokenString = tokenHandler.WriteToken(JwtToken);
            return serializeTokenString;
        }

        public string GenerateRefreshToken(int milisecondExpired)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMilliseconds(milisecondExpired),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.JwtSecret)), SecurityAlgorithms.HmacSha256),
                TokenType = "Refresh JWT Token"
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            string serializeTokenString = tokenHandler.WriteToken(jwtToken);
            return serializeTokenString;
        }
        public async Task<ClaimsPrincipal> ReadAndValidateTokenAsync(string token)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateIssuer = true;
            validationParameters.ValidateAudience = true;
            validationParameters.ValidateLifetime = false;
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidAudience = _jwtOptions.JwtValidAudience;
            validationParameters.ValidIssuer = _jwtOptions.JwtValidIssuer;
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtSecret));

            SecurityToken securityToken = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw LogHelper.LogArgumentNullException("token");
                }

                if (token.Length > tokenHandler.MaximumTokenSizeInBytes)
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("Token has length: '{0}' which is larger than the MaximumTokenSizeInBytes: '{1}'", token.Length, tokenHandler.MaximumTokenSizeInBytes)));
                }
                string[] array = token.Split(new char[1] { '.' }, 6);
                if (array.Length != 3 && array.Length != 5)
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(" JWT must have three segments (JWS) or five segments (JWE)."));
                }
                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken();
                Console.WriteLine("==== CHECK EXPIRE ====");
                if (jwtSecurityToken.ValidTo.CompareTo(DateTime.UtcNow) <= 0)
                {
                    Console.WriteLine("Datetime now: " + DateTime.UtcNow);
                    Console.WriteLine("Datetime valid to: " + jwtSecurityToken.ValidTo);
                    Console.WriteLine("==== COMPARE TO NOW ====" + (jwtSecurityToken.ValidTo.CompareTo(DateTime.UtcNow) <= 0));
                    Console.WriteLine("TOKEN EXPIRED NEED TO BE REFRESHED");
                    throw LogHelper.LogExceptionMessage(new SecurityTokenException("Token Expired"));

                }
                ClaimsPrincipal prinicpals = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                Console.WriteLine("==== EXISTED: " + prinicpals.Identity.Name);
                return prinicpals;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Stacktrace: " + ex.StackTrace);
                return null;
            }
        }

        public async Task<ClaimsPrincipal> GetPrincipalFromToken(string token)
        {

            try
            {
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Không kiểm tra thời gian sống của token
                    ValidateIssuerSigningKey = true,
                    ValidAudience = _jwtOptions.JwtValidAudience,
                    ValidIssuer = _jwtOptions.JwtValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtSecret))
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                if (tokenHandler.CanReadToken(token))
                {
                    SecurityToken securityToken;
                    ClaimsPrincipal principals = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                    Console.WriteLine("==== EXISTED: " + principals.Identity.Name);
                    return principals;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error validating token: " + ex.Message);
            }

            return null;
        }
    }
}
