using Infrastructure.Security.TokenProvider;
using Microsoft.Extensions.Options;

namespace Presentation.OptionsSetup
{
    public class JwtConfigOptionsSetup : IConfigureOptions<JwtConfigOptions>
    {
        private const string SectionName = "JWTConfig";
        private readonly IConfiguration _configuration;

        public JwtConfigOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(JwtConfigOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
            options.JwtSecret = _configuration.GetValue<string>("JWTConfig:JwtSecret");
            Console.WriteLine("SEECRETKEY ================= " + options.JwtSecret);

        }
    }
}
