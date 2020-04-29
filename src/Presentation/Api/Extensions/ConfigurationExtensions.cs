namespace Api.Extensions
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static IConfigurationSection GetJwtSecretSection(this IConfiguration configuration)
            => configuration.GetSection("JwtSettings");

        public static IConfigurationSection GetRedisSection(this IConfiguration configuration)
            => configuration.GetSection("RedisCacheSettings");

        public static string GetSendGridApiKey(this IConfiguration configuration)
            => configuration.GetSection("SendGrid:ApiKey").Value;

        public static string GetCloudinaryCloudName(this IConfiguration configuration)
            => configuration.GetSection("Cloudinary:CloudName").Value;

        public static string GetCloudinaryApiKey(this IConfiguration configuration)
            => configuration.GetSection("Cloudinary:ApiKey").Value;

        public static string GetCloudinaryApiSecret(this IConfiguration configuration)
            => configuration.GetSection("Cloudinary:ApiSecret").Value;
    }
}