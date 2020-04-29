namespace Api.Common
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Application.AppSettingsModels;
    using Application.Common.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int cachingTimeInMinutes;

        public CachedAttribute(int cachingTimeInMinutes)
        {
            this.cachingTimeInMinutes = cachingTimeInMinutes;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheOptions>();

            if (!cacheSettings.Enabled)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            if (cachedResponse != null)
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromMinutes(this.cachingTimeInMinutes));
            }
        }

        private static string GenerateCacheKey(HttpRequest request)
        {
            var builder = new StringBuilder();

            builder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                builder.Append($"|{key}-{value}");
            }

            return builder.ToString();
        }
    }
}
