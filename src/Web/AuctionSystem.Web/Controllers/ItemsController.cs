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
        private readonly ICache cache;
        private readonly IItemsService itemsService;
        private readonly IUserService userService;

        public ItemsController(IItemsService itemsService, ICache cache, IUserService userService)
        {
            this.itemsService = itemsService;
            this.cache = cache;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userService
                .GetUserIdByUsernameAsync(this.HttpContext.User.Identity.Name);

            var serviceItems = await this.itemsService
                    .GetAllItemsForGivenUser<ItemIndexServiceModel>(user);
            if (serviceItems == null)
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                return this.View();
            }

            var items = serviceItems
                .Select(Mapper.Map<ItemIndexViewModel>)
                .ToList();

            return this.View(items);
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
                return this.RedirectToHome();
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
        public async Task<IActionResult> Edit(string id)
        {
            var serviceModel = await this.itemsService.GetByIdAsync<ItemEditServiceModel>(id);

            if (serviceModel == null ||
                serviceModel.UserUserName != this.User.Identity.Name &&
                !this.User.IsInRole(WebConstants.AdministratorRole))
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                return this.RedirectToHome();
            }

            var model = Mapper.Map<ItemEditBindingModel>(serviceModel);

            model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(string id, ItemEditBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();

                return this.View(model);
            }

            var serviceModel = await this.itemsService.GetByIdAsync<ItemEditServiceModel>(id);

            if (serviceModel == null ||
                serviceModel.UserUserName != this.User.Identity.Name &&
                !this.User.IsInRole(WebConstants.AdministratorRole))
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                return this.RedirectToHome();
            }

            serviceModel.Title = model.Title;
            serviceModel.Description = model.Description;
            serviceModel.StartingPrice = model.StartingPrice;
            serviceModel.MinIncrease = model.MinIncrease;
            serviceModel.StartTime = model.StartTime.ToUniversalTime();
            serviceModel.EndTime = model.EndTime.ToUniversalTime();
            serviceModel.SubCategoryId = model.SubCategoryId;

            bool success = await this.itemsService.UpdateAsync(serviceModel);

            if (!success)
            {
                this.ShowErrorMessage(NotificationMessages.ItemUpdateError);

                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();

                return this.View(model);
            }

            this.ShowSuccessMessage(NotificationMessages.ItemUpdated);

            return this.RedirectToAction("Details", new { id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var serviceItem = await this.itemsService
                .GetByIdAsync<ItemDetailsServiceModel>(id);
            if (serviceItem == null ||
                serviceItem.UserUserName != this.User.Identity.Name &&
                !this.User.IsInRole(WebConstants.AdministratorRole))
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);

                return this.RedirectToHome();
            }

            var item = Mapper.Map<ItemDetailsViewModel>(serviceItem);

            return this.View(item);
        }

        [ActionName(nameof(Delete))]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var serviceItem = await this.itemsService
                .GetByIdAsync<ItemDetailsServiceModel>(id);

            if (serviceItem == null ||
                serviceItem.UserUserName != this.User.Identity.Name &&
                !this.User.IsInRole(WebConstants.AdministratorRole))
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);

                return this.RedirectToHome();
            }

            var isDeleted = await this.itemsService
                .DeleteAsync(id);
            if (!isDeleted)
            {
                this.ShowErrorMessage(NotificationMessages.ItemDeletedError);
                return this.RedirectToAction(nameof(this.Delete), new { id });
            }

            this.ShowSuccessMessage(NotificationMessages.ItemDeletedSuccessfully);
            return this.RedirectToAction(nameof(this.Index));
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
