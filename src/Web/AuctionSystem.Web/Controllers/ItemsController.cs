namespace AuctionSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Services.Interfaces;
    using Services.Models.Item;
    using Services.Models.SubCategory;
    using ViewModels.Item;

    public class ItemsController : Controller
    {
        private readonly IItemsService itemsService;
        private readonly ICategoriesService categoriesService;

        public ItemsController(IItemsService itemsService, ICategoriesService categoriesService)
        {
            this.itemsService = itemsService;
            this.categoriesService = categoriesService;
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

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = new ItemCreateBindingModel
            {
                SubCategories = await this.GetAllSubCategories()
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ItemCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.SubCategories = await this.GetAllSubCategories();

                return this.View(model);
            }

            var serviceModel = Mapper.Map<ItemCreateServiceModel>(model);

            serviceModel.UserName = this.User.Identity.Name;

            var id = await this.itemsService.CreateAsync(serviceModel);

            if (id == null)
            {
                //TODO: Add notification

                model.SubCategories = await this.GetAllSubCategories();

                return this.View(model);
            }

            return this.RedirectToAction("Details", new {id});
        }

        // Get all SubCategories and add them into SelectListGroups based on their parent categories
        private async Task<IEnumerable<SelectListItem>> GetAllSubCategories()
        {
            var subCategories = (await this.categoriesService
                    .GetAllSubCategoriesAsync<SubCategoryListingServiceModel>())
                .OrderBy(c => c.Category.Name)
                .ThenBy(c => c.Name)
                .ToArray();

            var selectListItems = new SelectListItem[subCategories.Length];

            var selectListGroups = new Dictionary<string, SelectListGroup>();

            for (int i = 0; i < subCategories.Length; i++)
            {
                var subCategory = subCategories[i];

                var categoryName = subCategory.Category.Name;

                var exists = selectListGroups.TryGetValue(categoryName, out var selectListGroup);

                if (!exists)
                {
                    selectListGroup = new SelectListGroup
                    {
                        Name = categoryName
                    };

                    selectListGroups.Add(categoryName, selectListGroup);
                }

                var item = new SelectListItem
                {
                    Text = subCategory.Name,
                    Value = subCategory.Id,
                    Group = selectListGroup
                };

                selectListItems[i] = item;
            }

            return selectListItems;
        }
    }
}