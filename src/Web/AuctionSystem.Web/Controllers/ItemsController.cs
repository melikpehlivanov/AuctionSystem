namespace AuctionSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using Services.Models.Item;
    using ViewModels.Item;

    public class ItemsController : Controller
    {
        private readonly IItemsService itemsService;

        public ItemsController(IItemsService itemsService)
        {
            this.itemsService = itemsService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var serviceModel = await this.itemsService.GetByIdAsync<ItemDetailsServiceModel>(id);

            if (serviceModel == null)
            {
                return this.NotFound();
            }

            var viewModel = Mapper.Map<ItemDetailsViewModel>(serviceModel);

            return this.View(viewModel);
        }
    }
}