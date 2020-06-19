namespace Application.AppSettingsModels
{
    public class RedisCacheOptions
    {
        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }
    }
}