namespace Common
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");

        public static IConfigurationSection GetRedisSection(this IConfiguration configuration)
            => configuration.GetSection("RedisCacheSettings");
    }
}