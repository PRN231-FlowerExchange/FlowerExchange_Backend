namespace Domain.Constants
{
    public static class TokenConstants
    {
        public const int ACCESS_TOKEN_PERIOD_MINISECOND = 60 * 1000;
        public const int REFRESH_TOKEN_PERIOD_MINISECOND = 60 * 60 * 1000;
        public const string TOKEN_LOGIN_PROVIDER_NAME = "JWT Authentication";
        public const string REFRESH_TOKEN_NAME = "RefreshToken";
    }
}
