namespace AuctionSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Middleware;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddDefaultSecurityHeaders(this IApplicationBuilder app, SecurityHeadersBuilder builder)
            => app.UseMiddleware<SecurityHeadersMiddleware>(builder.Policy());
    }
}