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
    using Services.Models.Category;
    using Services.Models.Item;
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
            IEnumerable<ItemListingServiceModel> serviceItems;

            if (id == null)
            {
                serviceItems = await this.itemsService
                    .GetAllItemsAsync<ItemListingServiceModel>();
            }
            else
            {
                serviceItems = await this.itemsService
                    .GetAllItemsInGivenCategoryByCategoryIdAsync<ItemListingServiceModel>(id);
            }

            if (!serviceItems.Any())
            {
                return RedirectToHome();
            }

            var allItems = serviceItems.Select(Mapper.Map<ItemListingDto>).ToList();

            var totalPages = (int)(Math.Ceiling(allItems.Count() / (double)WebConstants.ItemsCountPerPage));
            pageIndex = Math.Min(Math.Max(1, pageIndex), totalPages);

            var itemsToShow = allItems
                .Skip((pageIndex - 1) * WebConstants.ItemsCountPerPage)
                .Take(WebConstants.ItemsCountPerPage)
                .ToList();

            var items = new ItemListingViewModel { Items = new PaginatedList<ItemListingDto>(itemsToShow, pageIndex, totalPages) };
            return this.View(items);
        }

        public async Task<IActionResult> Details(string id)
        {
            var serviceModel = await this.itemsService.GetByIdAsync<ItemDetailsServiceModel>(id);

            if (serviceModel == null)
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                return this.RedirectToHome();
            }

            var viewModel = Mapper.Map<ItemDetailsViewModel>(serviceModel);

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = new ItemCreateBindingModel
            {
                SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync()
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ItemCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();

                return this.View(model);
            }

            var serviceModel = Mapper.Map<ItemCreateServiceModel>(model);
            serviceModel.StartTime = serviceModel.StartTime.ToUniversalTime();
            serviceModel.EndTime = serviceModel.EndTime.ToUniversalTime();
            serviceModel.UserName = this.User.Identity.Name;

            var id = await this.itemsService.CreateAsync(serviceModel);

            if (id == null)
            {
                this.ShowErrorMessage(NotificationMessages.ItemCreateError);

                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();

                return this.View(model);
            }

            this.ShowSuccessMessage(NotificationMessages.ItemCreated);

            return this.RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Search(string query, int pageIndex = 1)
        {
            query = query?.Trim();

            if (query == null || query.Length < 3)
            {
                this.ShowErrorMessage(NotificationMessages.SearchQueryTooShort);

                return this.RedirectToHome();
            }

            var serviceItems = (await this.itemsService
                    .SearchByTitleAsync<ItemListingServiceModel>(query))
                .ToArray();

            if (!serviceItems.Any())
            {
                this.ShowErrorMessage(NotificationMessages.SearchNoItems);

                return this.RedirectToHome();
            }

            var allItems = serviceItems.Select(Mapper.Map<ItemListingDto>).ToList();

            var totalPages = (int)Math.Ceiling(allItems.Count / (double)WebConstants.ItemsCountPerPage);
            pageIndex = Math.Min(Math.Max(1, pageIndex), totalPages);

            var itemsToShow = allItems
                .Skip((pageIndex - 1) * WebConstants.ItemsCountPerPage)
                .Take(WebConstants.ItemsCountPerPage)
                .ToList();

            var items = new ItemSearchViewModel
            {
                Items = new PaginatedList<ItemListingDto>(itemsToShow, pageIndex, totalPages),
                Query = query
            };

            this.ViewData[WebConstants.SearchViewDataKey] = query;

            return this.View(items);
        }
        
        private async Task<IEnumerable<SelectListItem>> GetAllCategoriesWithSubCategoriesAsync()
        {
            var categories = (await this.categoriesService
                    .GetAllCategoriesWithSubCategoriesAsync<CategoryListingServiceModel>())
                .OrderBy(c => c.Name)
                .ToArray();

            var selectListItems = new List<SelectListItem>();

            foreach (var category in categories)
            {
                var group = new SelectListGroup {Name = category.Name};
                foreach (var subCategory in category.SubCategories.OrderBy(c=> c.Name))
                {
                    var item = new SelectListItem
                    {
                        Text = subCategory.Name,
                        Value = subCategory.Id,
                        Group = group
                    };

                    selectListItems.Add(item);
                }
            }

            return selectListItems;
        }
    }
}
