namespace AuctionSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Application.Items.Commands.CreateItem;
    using Application.Items.Commands.DeleteItem;
    using Application.Items.Commands.UpdateItem;
    using Application.Items.Queries.Details;
    using Application.Items.Queries.List;
    using AutoMapper;
    using Infrastructure.Collections.Interfaces;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ViewModels.Item;

    public class ItemsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICache cache;
        private readonly ICurrentUserService currentUserService;

        public ItemsController(
            IMapper mapper,
            ICache cache,
            ICurrentUserService currentUserService)
        {
            this.mapper = mapper;
            this.cache = cache;
            this.currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await this.Mediator.Send(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    UserId = this.currentUserService.UserId

                }
            });

            if (response.Data == null)
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                return this.View();
            }

            var items = response
                .Data
                .Select(this.mapper.Map<ItemIndexViewModel>)
                .ToList();

            return this.View(items);
        }

        public async Task<IActionResult> List(Guid id, int pageIndex = 1)
        {
            IEnumerable<ListItemsResponseModel> items;

            if (id.Equals(Guid.Empty))
            {
                var response = await this.Mediator.Send(new ListItemsQuery());
                items = response.Data;
            }
            else
            {
                var response = await this.Mediator.Send(new ListItemsQuery
                {
                    Filters = new ListAllItemsQueryFilter
                    {
                        SubCategoryId = id,

                    }
                });
                items = response.Data;
            }

            if (!items.Any())
            {
                return this.RedirectToHome();
            }

            //TODO: Could use the returned pagination
            var allItems = items.Select(this.mapper.Map<ItemListingDto>)
                .ToPaginatedList(pageIndex, WebConstants.ItemsCountPerPage);

            var vm = new ItemListingViewModel { Items = allItems };
            return this.View(vm);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var response = await this.Mediator.Send(new GetItemDetailsQuery(id));
                var viewModel = this.mapper.Map<ItemDetailsViewModel>(response.Data);

                return this.View(viewModel);
            }
            catch (NotFoundException)
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                return this.RedirectToHome();
            }
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

            var appModel = this.mapper.Map<CreateItemCommand>(model);
            appModel.StartTime = appModel.StartTime.ToUniversalTime();
            appModel.EndTime = appModel.EndTime.ToUniversalTime();
            try
            {
                var result = await this.Mediator.Send(appModel);
                this.ShowSuccessMessage(NotificationMessages.ItemCreated);
                return this.RedirectToAction("Details", new { result.Data.Id });
            }
            catch (BadRequestException ex)
            {
                this.ShowErrorMessage(ex.Message);
                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();
                return this.View(model);
            }
            catch (NotFoundException ex)
            {
                this.ShowErrorMessage(ex.Message);
                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();
                return this.View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var response = await this.Mediator.Send(new GetItemDetailsQuery(id));

                if (response.Data.UserId != this.currentUserService.UserId &&
                    !this.currentUserService.IsAdmin)
                {
                    this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                    return this.RedirectToHome();
                }

                var model = this.mapper.Map<ItemEditBindingModel>(response.Data);
                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();

                return this.View(model);
            }
            catch (NotFoundException)
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                return this.RedirectToHome();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, ItemEditBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();

                return this.View(model);
            }

            try
            {
                var itemDetails = await this.Mediator.Send(new GetItemDetailsQuery(id));
                if (itemDetails.Data.UserId != this.currentUserService.UserId
                    && !this.currentUserService.IsAdmin)
                {
                    this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                    return this.RedirectToHome();
                }

                var appUpdateModel = this.mapper.Map<UpdateItemCommand>(model);
                appUpdateModel.StartTime = model.StartTime.ToUniversalTime();
                appUpdateModel.EndTime = model.EndTime.ToUniversalTime();

                await this.Mediator.Send(appUpdateModel);

                this.ShowSuccessMessage(NotificationMessages.ItemUpdated);
                return this.RedirectToAction("Details", new { id });
            }
            catch (NotFoundException)
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                return this.RedirectToHome();
            }
            catch (BadRequestException)
            {
                this.ShowErrorMessage(NotificationMessages.ItemUpdateError);
                model.SubCategories = await this.GetAllCategoriesWithSubCategoriesAsync();
                return this.View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await this.Mediator.Send(new GetItemDetailsQuery(id));
                if (response.Data.UserId != this.currentUserService.UserId
                    && !this.currentUserService.IsAdmin)
                {
                    this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                    return this.RedirectToHome();
                }

                var item = this.mapper.Map<ItemDetailsViewModel>(response.Data);
                return this.View(item);
            }
            catch (NotFoundException)
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                return this.RedirectToHome();
            }
        }

        [ActionName(nameof(Delete))]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var response = await this.Mediator.Send(new GetItemDetailsQuery(id));
                if (response.Data.UserId != this.currentUserService.UserId
                    && !this.currentUserService.IsAdmin)
                {
                    this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                    return this.RedirectToHome();
                }

                await this.Mediator.Send(new DeleteItemCommand(id));

                this.ShowSuccessMessage(NotificationMessages.ItemDeletedSuccessfully);
                return this.RedirectToAction(nameof(this.Index));
            }
            catch (NotFoundException)
            {
                this.ShowErrorMessage(NotificationMessages.ItemNotFound);
                return this.RedirectToHome();
            }
        }

        public async Task<IActionResult> Search(string query, int pageIndex = 1)
        {
            query = query?.Trim();

            if (query == null || query.Length < 3)
            {
                this.ShowErrorMessage(NotificationMessages.SearchQueryTooShort);

                return this.RedirectToHome();
            }

            var appModel = await this.Mediator.Send(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    Title = query,
                }
            });

            if (!appModel.Data.Any())
            {
                this.ShowErrorMessage(NotificationMessages.SearchNoItems);
                return this.RedirectToHome();
            }

            var allItems = appModel.Data.Select(this.mapper.Map<ItemListingDto>)
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
                        Value = subCategory.Id.ToString(),
                        Group = group
                    };

                    selectListItems.Add(item);
                }
            }

            return selectListItems;
        }
    }
}
