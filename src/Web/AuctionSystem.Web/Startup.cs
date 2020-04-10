namespace AuctionSystem.Web
{
    using System;
    using AutoMapper;
    using Common.AutoMapping.Profiles;
    using Common.EmailSender;
    using Data;
    using Infrastructure.Extensions;
    using Infrastructure.Middleware;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services.Models;
    using SignalRHubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<CloudinaryOptions>(options =>
                {
                    options.CloudName = this.Configuration.GetSection("Cloudinary:CloudName").Value;
                    options.ApiKey = this.Configuration.GetSection("Cloudinary:ApiKey").Value;
                    options.ApiSecret = this.Configuration.GetSection("Cloudinary:ApiSecret").Value;
                })
                .Configure<SendGridOptions>(options =>
                {
                    options.SendGridApiKey = this.Configuration.GetSection("SendGrid:ApiKey").Value;
                })
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services
                .Configure<CookieTempDataProviderOptions>(options => { options.Cookie.IsEssential = true; });

            services
                .AddDbContext<AuctionSystemDbContext>(options => options
                    .UseSqlServer(this.Configuration.GetDefaultConnectionString()))
                .AddIdentity();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddRazorPages();

            services
                .AddDistributedMemoryCache();

            services
                .AddDomainServices()
                .AddApplicationServices()
                .AddCommonProjectServices()
                .AddAuthentication();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            services
                .Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services
                .AddSignalR();

            services
                .AddResponseCompression(options => options.EnableForHttps = true);

            services.AddAutoMapper(typeof(DefaultProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.AddDefaultSecurityHeaders(
                new SecurityHeadersBuilder()
                    .AddDefaultSecurePolicy());
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BidHub>("/bidHub");
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "items",
                    pattern: "Items/{action}/{id}/{slug:required}",
                    defaults: new { controller = "Items", action = "Details" });
                endpoints.MapRazorPages();
            });

            app.SeedData();
        }
    }
}