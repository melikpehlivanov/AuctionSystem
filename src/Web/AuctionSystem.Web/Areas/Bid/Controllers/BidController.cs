namespace AuctionSystem.Web.Areas.Bid.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Interfaces;
    using Services.Models.Item;
    using Web.Controllers;

    [Area("Bid")]
    public class BidController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IBidService bidService;
        private readonly IItemsService itemsService;
        private readonly IUserService userService;

        public BidController(IMapper mapper, IBidService bidService, IItemsService itemsService, IUserService userService)
        {
            this.mapper = mapper;
            this.bidService = bidService;
            this.itemsService = itemsService;
            this.userService = userService;
        }

        [Route("/bid/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                this.ShowErrorMessage(NotificationMessages.BidNotFound);
                return this.RedirectToHome();
            }

            var serviceModel = await this.itemsService.GetByIdAsync<ItemDetailsServiceModel>(id);
            var highestBid = await this.bidService.GetHighestBidAmountForGivenItemAsync(id);

            var viewModel = this.mapper.Map<BidDetailsViewModel>(serviceModel);
            viewModel.UserId = await this.userService.GetUserIdByUsernameAsync(this.User.Identity.Name);
            viewModel.ReturnUrl = this.HttpContext.Request.Path.ToString();
            viewModel.HighestBid = highestBid ?? 0;

            return this.View(viewModel);
        }
    }
}
