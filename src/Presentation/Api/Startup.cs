namespace Api
{
    using Application;
    using Application.Users.Commands.CreateUser;
    using AuctionSystem.Infrastructure;
    using Extensions;
    using FluentValidation.AspNetCore;
    using Hubs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Middlewares;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Persistence;
    using Services.Hosted;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            services
                .AddPersistence(this.Configuration)
                .AddHostedService<MigrateDatabaseHostedService>()
                .AddInfrastructure(this.Configuration)
                .AddApplication()
                .AddCloudinarySettings(this.Configuration)
                .AddSendGridSettings(this.Configuration)
                .AddJwtAuthentication(services.AddJwtSecret(this.Configuration))
                .AddRequiredServices()
                .AddRedisCache(this.Configuration)
                .AddSwagger()
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:3000",
                                "https://localhost:3000");
                            builder.AllowCredentials();
                            builder.AllowAnyMethod();
                            builder.AllowAnyHeader();
                        });
                })
                .AddControllers()
                .AddNewtonsoftJson(options => options.UseCamelCasing(true))
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>());

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection()
                .UseCustomExceptionHandler()
                .UseAuthorizationHeader()
                .UseRouting()
                .UseHsts()
                .UseCors()
                .UseAuthentication()
                .UseAuthorization()
                .UseSwaggerUi()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<BidHub>("/bidHub");
                });
        }
    }
}