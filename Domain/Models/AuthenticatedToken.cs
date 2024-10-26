namespace Domain.Models
{
    public class AuthenticatedToken
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? TokenType { get; set; }
    }
}
