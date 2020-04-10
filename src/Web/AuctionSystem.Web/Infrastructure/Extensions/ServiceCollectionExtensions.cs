namespace AuctionSystem.Web.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Common.EmailSender;
    using Common.EmailSender.Interface;
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Services.Interfaces;
    using Services.Models;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(IService));

            AddAssemblyServices(services, assembly);

            return services;
        }

        public static IServiceCollection AddCommonProjectServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(IEmailSender));
            AddAssemblyServices(services, assembly);

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            AddAssemblyServices(services, assembly, true);

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AuctionUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<AuctionSystemDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }

        public static IServiceCollection AddAppSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .Configure<CloudinaryOptions>(options =>
                {
                    options.CloudName = configuration.GetSection("Cloudinary:CloudName").Value;
                    options.ApiKey = configuration.GetSection("Cloudinary:ApiKey").Value;
                    options.ApiSecret = configuration.GetSection("Cloudinary:ApiSecret").Value;
                })
                .Configure<SendGridOptions>(options => { options.SendGridApiKey = configuration.GetSection("SendGrid:ApiKey").Value; });

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
                .Configure<SecurityStampValidatorOptions>(options =>
                {
                    options.ValidationInterval = TimeSpan.Zero;
                });

            return services;
        }

        private static void AddAssemblyServices(IServiceCollection services, Assembly assembly, bool isApp = false)
        {
            var servicesToRegister = assembly
                .GetTypes()
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.GetInterfaces()
                                .Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ToList();

            if (isApp)
            {
                servicesToRegister.ForEach(s => services.AddTransient(s.Interface, s.Implementation));
            }
            else
            {
                servicesToRegister.ForEach(s => services.AddScoped(s.Interface, s.Implementation));
            }
        }
    }
}
