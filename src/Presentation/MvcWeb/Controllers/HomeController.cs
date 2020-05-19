namespace MvcWeb.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Items.Queries.List;
    using AutoMapper;
    using Infrastructure.Collections.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;
    using ViewModels.Item;

    public class HomeController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICache cache;

        public HomeController(IMapper mapper, ICache cache)
        {
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var hottestItemsResponse = await this.Mediator.Send(new ListItemsQuery()
            {
                Filters = new ListAllItemsQueryFilter
                {
                    MinPrice = 10000,
                    StartTime = DateTime.UtcNow,
                }
            });
            var liveItemsResponse = await this.Mediator.Send(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    GetLiveItems = true,
                    MinimumPicturesCount = 2,
                }
            });

            var liveItems = hottestItemsResponse.Data.Select(this.mapper.Map<LiveItemViewModel>);
            var hottestItems = liveItemsResponse.Data.Select(this.mapper.Map<HottestItemViewModel>);

            var model = new HomeViewModel
            {
                LiveItems = liveItems,
                HottestItems = hottestItems,
                Categories = await this.cache.GetAllCategoriesWithSubcategoriesAsync()
            };

            return this.View(model);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }
    }
}
