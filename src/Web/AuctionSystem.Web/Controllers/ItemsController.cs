namespace AuctionSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Collections;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Services.Interfaces;
    using Services.Models.Item;
    using Services.Models.SubCategory;
    using ViewModels.Item;

    public class ItemsController : BaseController
    {
        private readonly IItemsService itemsService;
        private readonly ICategoriesService categoriesService;

        public ItemsController(IItemsService itemsService, ICategoriesService categoriesService)
        {
            this.itemsService = itemsService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> List(string id, int pageIndex = 1)
        {
            IEnumerable<ItemListingServiceModel> allItems;

            if (id == null)
            {
                allItems = await this.itemsService
                    .GetAllItems<ItemListingServiceModel>();
            }
            else
            {
                allItems = await this.itemsService
                    .GetAllItemsInGivenCategoryByCategoryIdAsync<ItemListingServiceModel>(id);
            }

            if (!allItems.Any())
            {
                return this.NotFound();
            }

            var totalPages = (int)(Math.Ceiling(allItems.Count() / (double)WebConstants.ItemsCountPerPage));
            pageIndex = Math.Min(Math.Max(1, pageIndex), totalPages);

            var itemsToShow = allItems
                .Skip((pageIndex - 1) * WebConstants.ItemsCountPerPage)
                .Take(WebConstants.ItemsCountPerPage)
                .ToList();

            var items = new ItemListingViewModel { Items = new PaginatedList<ItemListingServiceModel>(itemsToShow, pageIndex, totalPages) };
            return this.View(items);
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
                SubCategories = await this.GetAllSubCategoriesAsync()
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ItemCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.SubCategories = await this.GetAllSubCategoriesAsync();

                return this.View(model);
            }

            var serviceModel = Mapper.Map<ItemCreateServiceModel>(model);

            serviceModel.UserName = this.User.Identity.Name;

            var id = await this.itemsService.CreateAsync(serviceModel);

            if (id == null)
            {
                this.ShowErrorMessage(NotificationMessages.ItemCreateError);

                model.SubCategories = await this.GetAllSubCategoriesAsync();

                return this.View(model);
            }
            
            this.ShowSuccessMessage(NotificationMessages.ItemCreated);

            return this.RedirectToAction("Details", new { id });
        }

        // Get all SubCategories and add them into SelectListGroups based on their parent categories
        private async Task<IEnumerable<SelectListItem>> GetAllSubCategoriesAsync()
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
