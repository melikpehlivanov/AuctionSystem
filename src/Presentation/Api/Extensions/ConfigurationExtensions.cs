namespace Api.Extensions
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");

        public static string GetCloudinaryCloudName(this IConfiguration configuration)
            => configuration.GetSection("Cloudinary:CloudName").Value;

        public static string GetCloudinaryApiKey(this IConfiguration configuration)
            => configuration.GetSection("Cloudinary:ApiKey").Value;

        public static string GetCloudinaryApiSecret(this IConfiguration configuration)
            => configuration.GetSection("Cloudinary:ApiSecret").Value;
    }
}
