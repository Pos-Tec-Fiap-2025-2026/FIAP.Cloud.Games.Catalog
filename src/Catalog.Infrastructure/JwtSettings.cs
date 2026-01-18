namespace Catalog.Infrastructure
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; }
    }
}
