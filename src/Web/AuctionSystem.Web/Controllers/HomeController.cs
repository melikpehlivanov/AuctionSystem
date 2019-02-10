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
        private readonly ICache cache;
        private readonly IItemsService itemsService;

        public HomeController(IItemsService itemsService, ICache cache)
        {
            this.itemsService = itemsService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var serviceModelHottestItems = await this.itemsService.GetHottestItemsAsync<HottestItemServiceModel>();
            var serviceLiveItems = await this.itemsService.GetAllLiveItemsAsync<LiveItemServiceModel>();

            var liveItems = serviceLiveItems.Select(Mapper.Map<LiveItemViewModel>);
            var hottestItems = serviceModelHottestItems.Select(Mapper.Map<HottestItemViewModel>);

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
