namespace Api.Middlewares
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;

    public class AuthorizationHeaderMiddleware
    {
        private readonly RequestDelegate next;
        private const string AuthorizationHeader = "Authorization";

        public AuthorizationHeaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Cookies.TryGetValue(ApiConstants.RefreshToken, out var refreshToken);
            context.Request.Cookies.TryGetValue(ApiConstants.JwtToken, out var jwtToken);

            if (jwtToken != null && refreshToken != null)
            {
                context.Request.Headers.Append(AuthorizationHeader, $"Bearer {jwtToken}");
            }

            await this.next(context);
        }
    }

    public static class AuthorizationHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationHeader(this IApplicationBuilder builder)
            => builder.UseMiddleware<AuthorizationHeaderMiddleware>();
    }
}