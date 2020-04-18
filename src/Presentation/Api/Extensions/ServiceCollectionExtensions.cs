namespace Api.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Application.AppSettingsModels;
    using Application.Common.Interfaces;
    using AuctionSystem.Infrastructure;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Services;
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.Swagger;

    public static class ServiceCollectionExtensions
    {
        public static AppSettings AddJwtSecret(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetJwtSecretSection();
            services.Configure<AppSettings>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppSettings>();
        }

        public static IServiceCollection AddCloudinarySettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .Configure<CloudinaryOptions>(options =>
                {
                    options.CloudName = configuration.GetCloudinaryCloudName();
                    options.ApiKey = configuration.GetCloudinaryApiKey();
                    options.ApiSecret = configuration.GetCloudinaryApiSecret();
                });

            return services;
        }

        public static IServiceCollection AddSendGridSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .Configure<SendGridOptions>(options => { options.SendGridApiKey = configuration.GetSendGridApiKey(); });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddRequiredServices(this IServiceCollection services)
            => services
                .AddScoped<ICurrentUserService, CurrentUserService>();

        public static IServiceCollection AddSwagger(this IServiceCollection services)
            => services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "AuctionSystem API",
                            Version = "v1"
                        });

                    c.ExampleFilters();
                    c.EnableAnnotations();

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = @"<h3>JWT Authorization header using the Bearer scheme.</h3>
                        <p>Enter 'Bearer' [space] and then your token in the text input below.</p>
                        <p>Example: Bearer 12345abcdef</p>",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    c.AddFluentValidationRules();
                })
                .AddSwaggerExamplesFromAssemblyOf<Startup>();
    }
}