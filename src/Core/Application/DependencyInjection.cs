namespace Application
{
    using System.Reflection;
    using AutoMapper;
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
            // TODO: Add behaviour later on
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}
