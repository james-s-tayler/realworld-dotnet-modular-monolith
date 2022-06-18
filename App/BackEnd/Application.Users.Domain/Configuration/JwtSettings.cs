namespace Application.Users.Domain.Configuration
{
    internal class JwtSettings
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}