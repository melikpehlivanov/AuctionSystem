namespace AuctionSystem.Web.Infrastructure.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Extensions;
    using Interfaces;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;
    using Services.Interfaces;
    using Services.Models.Category;
    using ViewModels.Category;

    public class Cache : ICache
    {
        private const string CategoriesKey = "_CategoriesStoredInCache";

        private readonly IMapper mapper;
        private readonly IDistributedCache cache;
        private readonly ICategoriesService categoriesService;

        public Cache(IMapper mapper, ICategoriesService categoriesService, IDistributedCache cache)
        {
            this.mapper = mapper;
            this.categoriesService = categoriesService;
            this.cache = cache;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesWithSubcategoriesAsync()
        {
            IEnumerable<CategoryViewModel> list;

            var cacheList = await this.cache.GetStringAsync(CategoriesKey);
            if (cacheList == null)
            {
                var serviceCategories = await this.categoriesService.GetAllCategoriesWithSubCategoriesAsync<CategoryListingServiceModel>();
                var categories = serviceCategories
                    .Select(this.mapper.Map<CategoryViewModel>)
                    .ToList();

                foreach (var category in categories)
                {
                    category.SubCategories = category.SubCategories.OrderBy(c => c.Name);
                }

                list = categories;
                var expiration = TimeSpan.FromDays(WebConstants.StaticElementsExpirationTimeInDays);
                await this.cache.SetCacheAsync(CategoriesKey, list, expiration);
            }
            else
            {
                list = JsonConvert.DeserializeObject<IEnumerable<CategoryViewModel>>(cacheList);

                return list;
            }

            return list;
        }
    }
}
