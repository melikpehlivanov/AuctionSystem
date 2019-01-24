namespace AuctionSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using Services.Models.Category;
    using Services.Models.Item;
    using ViewModels;
    using ViewModels.Category;
    using ViewModels.Item;

    public class HomeController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IItemsService itemsService;

        public HomeController(ICategoriesService categoriesService, IItemsService itemsService)
        {
            this.categoriesService = categoriesService;
            this.itemsService = itemsService;
        }

        public async Task<IActionResult> Index()
        {
            var serviceModelHottestItems = await this.itemsService.GetHottestItems<HottestItemServiceModel>();
            var hottestItems = serviceModelHottestItems.Select(Mapper.Map<HottestItemViewModel>);

            var model = new HomeViewModel
            {
                HottestItems = hottestItems,
                Categories = await this.GetAllCategoriesWithSubCategoriesAsync()
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

        private async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesWithSubCategoriesAsync()
        {
            var categories = (await this.categoriesService
                    .GetAllCategoriesWithSubCategoriesAsync<CategoryListingServiceModel>())
                .OrderBy(c => c.Name)
                .Select(Mapper.Map<CategoryViewModel>)
                .ToArray();

            foreach (var category in categories)
            {
                category.SubCategories = category.SubCategories.OrderBy(c => c.Name);
            }
            
            return categories;

        }
    }
}
