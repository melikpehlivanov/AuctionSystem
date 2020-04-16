namespace Api
{
    using Application;
    using Application.Common.Interfaces;
    using Application.Users.Commands.CreateUser;
    using AuctionSystem.Infrastructure;
    using Common;
    using Extensions;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Persistance;
    using Services;

    public class Startup
    {
        public IWebHostEnvironment Environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public IConfiguration Configuration { get; }

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
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IUriService>(provider =>
                {
                    var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                    var request = accessor.HttpContext.Request;
                    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), request.Path);
                    return new UriService(absoluteUri);
                })
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
                .UseAuthentication()
                .UseAuthorization()
                .UseSwaggerUi()
                //Allow anything for now
                .UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod())
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}