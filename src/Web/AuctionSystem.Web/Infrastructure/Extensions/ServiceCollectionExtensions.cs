namespace AuctionSystem.Web.Infrastructure.Extensions
{
    using System;
    using Application.AppSettingsModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .Configure<CloudinaryOptions>(options =>
                {
                    options.CloudName = configuration.GetCloudinaryCloudName();
                    options.ApiKey = configuration.GetCloudinaryApiKey();
                    options.ApiSecret = configuration.GetCloudinaryApiSecret();
                })
                .Configure<SendGridOptions>(options => { options.SendGridApiKey = configuration.GetSendgridApiKey(); });

            return services;
        }

        public static IServiceCollection ConfigureCookies(
            this IServiceCollection services)
        {
            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                })
                .Configure<CookieTempDataProviderOptions>(options => { options.Cookie.IsEssential = true; });

            return services;
        }

        public static IServiceCollection ConfigureSecurityStampValidator(this IServiceCollection services)
        {
            services
                .Configure<SecurityStampValidatorOptions>(options => { options.ValidationInterval = TimeSpan.Zero; });

            return services;
        }
    }
}