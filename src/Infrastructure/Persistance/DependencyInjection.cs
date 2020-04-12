namespace Persistance
{
    using Application.Common.Interfaces;
    using Common;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuctionSystemDbContext>(options =>
                options.UseSqlServer(configuration.GetDefaultConnectionString()));

            services.AddScoped<IAuctionSystemDbContext>(provider => provider.GetService<AuctionSystemDbContext>());

            return services;
        }
    }
}
