namespace Persistence
{
    using Application.Common.Interfaces;
    using AuctionSystem.Infrastructure.Identity;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<AuctionSystemDbContext>(options =>
                    options.UseSqlServer(configuration.GetDefaultConnectionString()))
                .AddIdentity<AuctionUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                    options.SignIn.RequireConfirmedEmail = true;
                    options.Lockout.MaxFailedAccessAttempts = 6;
                })
                .AddTokenProvider<FourDigitTokenProvider>(FourDigitTokenProvider.FourDigitEmail)
                .AddEntityFrameworkStores<AuctionSystemDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAuctionSystemDbContext>(provider => provider.GetService<AuctionSystemDbContext>());
            return services;
        }
    }
}