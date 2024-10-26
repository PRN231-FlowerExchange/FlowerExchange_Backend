namespace Infrastructure.Security.TokenProvider
{
    public class JwtConfigOptions
    {
        public string JwtSecret { get; set; }
        public string JwtValidIssuer { get; set; }
        public string JwtValidAudience { get; set; }
    }
}
