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
    using ViewModels;
    using ViewModels.Category;

    public class HomeController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public HomeController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetAllCategoriesWithSubCategoriesAsync());
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
                .ToArray()
                .Select(Mapper.Map<CategoryViewModel>);
            
            return categories;

        }
    }
}
