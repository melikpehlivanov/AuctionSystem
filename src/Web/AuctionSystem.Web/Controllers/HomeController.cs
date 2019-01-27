namespace AuctionSystem.Web.Controllers
{
    using System.Diagnostics;
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
        private readonly ICategoriesService categoriesService;
        private readonly IItemsService itemsService;
        private readonly ICache cache;

        public HomeController(ICategoriesService categoriesService, IItemsService itemsService, ICache cache)
        {
            this.categoriesService = categoriesService;
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
                Categories = await this.cache.GetAllCategoriesWithSubcategoriesAsync(),
            };
            
            return this.View(model);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
