using Domain.Constants;
using Infrastructure.Security.TokenProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Presentation.OptionsSetup
{
    public class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly JwtConfigOptions _jwtOptions;

        public JwtBearerOptionsSetup(IOptions<JwtConfigOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            options.TokenValidationParameters.ValidateIssuer = true;
            options.TokenValidationParameters.ValidateAudience = true;
            options.TokenValidationParameters.ValidateLifetime = true;
            options.TokenValidationParameters.ValidateIssuerSigningKey = true;
            options.TokenValidationParameters.ValidAudience = _jwtOptions.JwtValidAudience;
            options.TokenValidationParameters.ValidIssuer = _jwtOptions.JwtValidIssuer;
            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtSecret));
            options.TokenValidationParameters.RequireExpirationTime = true;
            options.TokenValidationParameters.RequireSignedTokens = true;
            options.Audience = _jwtOptions.JwtValidAudience;
            options.TokenValidationParameters.ClockSkew = TimeSpan.Zero; // No clock skew, strict expiration time validation
        }
    }
}
