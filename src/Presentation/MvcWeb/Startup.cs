namespace MvcWeb
{
    using Application;
    using Application.Common.Interfaces;
    using Application.Users.Commands.CreateUser;
    using AuctionSystem.Infrastructure;
    using AutoMapper;
    using Common.AutoMapping.Profiles;
    using FluentValidation.AspNetCore;
    using Infrastructure.Collections;
    using Infrastructure.Collections.Interfaces;
    using Infrastructure.Extensions;
    using Infrastructure.Middleware;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Persistance;
    using SignalRHubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddPersistence(this.Configuration)
                .AddInfrastructure(this.Configuration)
                .AddApplication()
                .AddAutoMapper(typeof(DefaultProfile))
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<ICache, Cache>()
                .AddScoped<IUriService>(provider =>
                {
                    var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                    var request = accessor.HttpContext.Request;
                    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), request.Path);
                    return new UriService(absoluteUri);
                })
                .AddTransient<ICurrentUserService, CurrentUserService>()
                .AddAppSettings(this.Configuration)
                .ConfigureCookies()
                .ConfigureSecurityStampValidator()
                .Configure<RouteOptions>(options => options.LowercaseUrls = true)
                .AddResponseCompression(options => options.EnableForHttps = true)
                .AddDistributedMemoryCache()
                .AddControllersWithViews(options => { options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>(); })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>());

            services.AddRazorPages();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
                app.UseHsts();
            }

            app.UseResponseCompression()
                .UseStatusCodePagesWithReExecute("/error/{0}")
                .AddDefaultSecurityHeaders(
                    new SecurityHeadersBuilder()
                        .AddDefaultSecurePolicy())
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<BidHub>("/bidHub");
                    endpoints.MapControllerRoute(
                        "areas",
                        "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute(
                        "default",
                        "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute(
                        "items",
                        "Items/{action}/{id}/{slug:required}",
                        new { controller = "Items", action = "Details" });
                    endpoints.MapRazorPages();
                });
        }
    }
}