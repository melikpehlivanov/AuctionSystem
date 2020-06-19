namespace Api.Services.Hosted
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.SeedSampleData;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class MigrateDatabaseHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public MigrateDatabaseHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var services = scope.ServiceProvider;
            try
            {
                var auctionSystemDbContext = services.GetRequiredService<AuctionSystemDbContext>();
                await auctionSystemDbContext.Database.MigrateAsync(cancellationToken);
                logger.LogInformation("Migrated database.");

                var mediator = services.GetRequiredService<IMediator>();
                logger.LogInformation("Seeding sample data such as items, categories and etc.");
                await mediator.Send(new SeedSampleDataCommand(), CancellationToken.None);
                logger.LogInformation("Database seeding was successful.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or initializing the database.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}