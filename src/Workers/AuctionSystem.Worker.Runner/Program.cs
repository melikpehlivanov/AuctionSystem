namespace AuctionSystem.Worker.Runner
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Application.AppSettingsModels;
    using Application.Common.Interfaces;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);

            var serviceProvider = services.BuildServiceProvider();
            var jobManager = serviceProvider.GetService<JobManager>();

            while (true)
            {
                Task.Run(async () => { await jobManager.ExecuteAllJobs(); }).Wait();
            }
        }

        private static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<AuctionSystemDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services
                .Configure<SendGridOptions>(options =>
                {
                    options.SendGridApiKey = configuration.GetSection("SendGrid:ApiKey").Value;
                });

            //TODO: Add azure app services logger when we publish this app
            services
                .AddLogging(configure => configure.AddConsole().AddFilter("Microsoft", LogLevel.None))
                .AddTransient<JobManager>();

            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}