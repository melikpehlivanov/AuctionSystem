namespace Api
{
    using Application;
    using Application.Users.Commands.CreateUser;
    using AuctionSystem.Infrastructure;
    using Common;
    using Extensions;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Persistence;

    public class Startup
    {
        private IWebHostEnvironment Environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
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
                .AddInfrastructure(this.Configuration)
                .AddApplication()
                .AddCloudinarySettings(this.Configuration)
                .AddSendGridSettings(this.Configuration)
                .AddJwtAuthentication(services.AddJwtSecret(this.Configuration))
                .AddRequiredServices()
                .AddRedisCache(this.Configuration)
                .AddSwagger()
                .AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>());
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
                .UseRouting()
                .UseHsts()
                .UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod())
                .UseAuthentication()
                .UseAuthorization()
                .UseSwaggerUi()
                //TODO: Allow only client app when it's implemented
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}