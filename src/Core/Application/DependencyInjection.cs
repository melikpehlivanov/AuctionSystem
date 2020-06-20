namespace Application
{
    using System.Reflection;
    using AutoMapper;
    using Common;
    using Common.Behaviours;
    using global::Common.AutoMapping.Profiles;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(DefaultProfile));
            services
                .AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddHostedService<EmailNotificationSenderService>();
            return services;
        }
    }
}