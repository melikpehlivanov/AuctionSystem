namespace AuctionSystem.Web.Infrastructure.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Categories.Queries.List;
    using Application.Common.Models;
    using AutoMapper;
    using Extensions;
    using Interfaces;
    using MediatR;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;
    using ViewModels.Category;

    public class Cache : ICache
    {
        private const string CategoriesKey = "_CategoriesStoredInCache";

        private readonly IMapper mapper;
        private readonly IDistributedCache cache;
        private readonly IMediator mediator;

        public Cache(IMapper mapper, IDistributedCache cache, IMediator mediator)
        {
            this.mapper = mapper;
            this.cache = cache;
            this.mediator = mediator;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesWithSubcategoriesAsync()
        {
            IEnumerable<CategoryViewModel> list;

            var cacheList = await this.cache.GetStringAsync(CategoriesKey);
            if (cacheList == null)
            {
                var categoriesResponse = await this.mediator.Send(new ListCategoriesQuery());
                var categories = categoriesResponse
                    .Data
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
