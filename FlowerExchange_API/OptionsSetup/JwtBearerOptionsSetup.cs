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
            options.TokenValidationParameters.ValidateIssuerSigningKey = false;
            options.TokenValidationParameters.ValidAudience = _jwtOptions.JwtValidAudience;
            options.TokenValidationParameters.ValidIssuer = _jwtOptions.JwtValidIssuer;
            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtSecret));
            options.TokenValidationParameters.RequireExpirationTime = true;
            options.TokenValidationParameters.RequireSignedTokens = true;
            //options.TokenValidationParameters.ClockSkew = TimeSpan.FromMilliseconds(TokenConstants.ACCESS_TOKEN_PERIOD_MINISECOND);
            options.Audience = _jwtOptions.JwtValidAudience;
            options.Authority = "https://login.microsoftonline.com/a1d50521-9687-4e4d-a76d-ddd53ab0c668/";
        }
    }
}
