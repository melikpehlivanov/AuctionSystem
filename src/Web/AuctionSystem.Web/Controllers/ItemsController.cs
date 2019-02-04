namespace AuctionSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Collections.Interfaces;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Services.Interfaces;
    using Services.Models.Item;
    using ViewModels.Item;

    public class ItemsController : BaseController
    {
        private readonly IItemsService itemsService;
        private readonly ICache cache;

        public ItemsController(IItemsService itemsService, ICache cache)
        {
            this.itemsService = itemsService;
            this.cache = cache;
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

            var allItems = serviceItems.Select(Mapper.Map<ItemListingDto>)
                .ToPaginatedList(pageIndex, WebConstants.ItemsCountPerPage);

            var items = new ItemListingViewModel { Items = allItems };
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

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var serviceItem = await this.itemsService
                .GetByIdAsync<ItemDetailsServiceModel>(id);
            if (serviceItem == null ||
                serviceItem.User.UserName != this.User.Identity.Name &&
                !this.User.IsInRole(WebConstants.AdministratorRole))
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);

                return RedirectToHome();
            }

            var item = Mapper.Map<ItemDetailsViewModel>(serviceItem);

            return View(item);
        }

        [ActionName(nameof(Delete))]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var serviceItem = await this.itemsService
                .GetByIdAsync<ItemDetailsServiceModel>(id);
            
            if (serviceItem == null ||
                serviceItem.User.UserName != this.User.Identity.Name &&
                !this.User.IsInRole(WebConstants.AdministratorRole))
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);

                return this.RedirectToHome();
            }
            
            var isDeleted = await this.itemsService
                .DeleteAsync(id);
            if (!isDeleted)
            {
                this.ShowSuccessMessage(NotificationMessages.ItemDeletedError);
                return this.RedirectToAction(nameof(Delete), new { id });
            }

            this.ShowErrorMessage(NotificationMessages.ItemDeletedSuccessfully);
            return this.RedirectToHome();
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

            var allItems = serviceItems.Select(Mapper.Map<ItemListingDto>)
                .ToPaginatedList(pageIndex, WebConstants.ItemsCountPerPage);

            var items = new ItemSearchViewModel
            {
                Items = allItems,
                Query = query
            };

            this.ViewData[WebConstants.SearchViewDataKey] = query;

            return this.View(items);
        }

        private async Task<IEnumerable<SelectListItem>> GetAllCategoriesWithSubCategoriesAsync()
        {
            var categories = await this.cache.GetAllCategoriesWithSubcategoriesAsync();

            var selectListItems = new List<SelectListItem>();

            foreach (var category in categories)
            {
                var group = new SelectListGroup { Name = category.Name };
                foreach (var subCategory in category.SubCategories.OrderBy(c => c.Name))
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
