namespace AuctionSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Collections.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using Services.Models.Item;
    using ViewModels;
    using ViewModels.Item;

    public class HomeController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICache cache;
        private readonly IItemsService itemsService;

        public HomeController(IMapper mapper, ICache cache, IItemsService itemsService)
        {
            this.mapper = mapper;
            this.cache = cache;
            this.itemsService = itemsService;
        }

        public async Task<IActionResult> Index()
        {
            var serviceModelHottestItems = await this.itemsService.GetHottestItemsAsync<HottestItemServiceModel>();
            var serviceLiveItems = await this.itemsService.GetAllLiveItemsAsync<LiveItemServiceModel>();

            var liveItems = serviceLiveItems.Select(this.mapper.Map<LiveItemViewModel>);
            var hottestItems = serviceModelHottestItems.Select(this.mapper.Map<HottestItemViewModel>);

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
