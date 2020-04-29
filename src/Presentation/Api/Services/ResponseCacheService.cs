namespace Api.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;

    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string key, object response, TimeSpan cachingTime)
        {
            if (response == null)
            {
                return;
            }

            await this.distributedCache
                .SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(response),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = cachingTime
                    });
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
            => await this.distributedCache.GetStringAsync(cacheKey);
    }
}
