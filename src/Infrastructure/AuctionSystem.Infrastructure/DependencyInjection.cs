namespace AuctionSystem.Infrastructure
{
    using Application.Common.Interfaces;
    using Common;
    using Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddScoped<IUserManager, UserManagerService>()
                .AddTransient<IDateTime, MachineDateTime>()
                .AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}