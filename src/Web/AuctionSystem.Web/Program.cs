namespace AuctionSystem.Web
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.SeedSampleData;
    using MediatR;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Persistance;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var services = scope.ServiceProvider;

                try
                {
                    var auctionSystemDbContext = services.GetRequiredService<AuctionSystemDbContext>();
                    auctionSystemDbContext.Database.Migrate();
                    logger.LogInformation("Migrated database.");

                    var mediator = services.GetRequiredService<IMediator>();
                    await mediator.Send(new SeedSampleDataCommand(), CancellationToken.None);
                    logger.LogInformation("Seeded sample data such as items, categories and etc.");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}