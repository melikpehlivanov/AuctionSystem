namespace Api
{
    using AuctionSystem.Infrastructure.Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Persistance;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedData(this IApplicationBuilder builder)
        {
            using (var serviceScope =
                builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AuctionSystemDbContext>();
                var applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                dbContext.Database.Migrate();
                applicationDbContext.Database.Migrate();
            }

            return builder;
        }
    }
}
