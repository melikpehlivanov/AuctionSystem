namespace Api.Extensions
{
    using AuctionSystem.Infrastructure.Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Persistance;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder builder)
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

        public static IApplicationBuilder UseSwaggerUi(this IApplicationBuilder app)
            => app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuctionSystem API");
                    options.RoutePrefix = string.Empty;
                });
    }
}
