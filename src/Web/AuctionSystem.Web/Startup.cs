namespace AuctionSystem.Web
{
    using AutoMapper;
    using Common.AutoMapping.Profiles;
    using Data;
    using Infrastructure.Extensions;
    using Infrastructure.Middleware;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
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
                .AddDbContext<AuctionSystemDbContext>(options => options
                    .UseSqlServer(this.Configuration.GetDefaultConnectionString()))
                .AddIdentity()
                .AddAppSettings(this.Configuration)
                .ConfigureCookies()
                .ConfigureSecurityStampValidator()
                .Configure<RouteOptions>(options => options.LowercaseUrls = true)
                .AddResponseCompression(options => options.EnableForHttps = true)
                .AddAutoMapper(typeof(DefaultProfile))
                .AddDistributedMemoryCache()
                .AddDomainServices()
                .AddApplicationServices()
                .AddCommonProjectServices()
                .AddAuthentication();

            services.AddControllersWithViews(options => { options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>(); });
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
                })
                .SeedData();
        }
    }
}