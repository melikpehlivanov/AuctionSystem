namespace AuctionSystem.Web.Areas.Bid.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Interfaces;
    using Services.Models.Item;

    [Area("Bid")]
    public class BidController : Controller
    {
        private readonly IItemsService itemsService;
        private readonly IBidService bidService;
        private readonly IUserService userService;

        public BidController(IItemsService itemsService, IBidService bidService, IUserService userService)
        {
            this.itemsService = itemsService;
            this.bidService = bidService;
            this.userService = userService;
        }

        [Route("/bid/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var serviceModel = await this.itemsService.GetByIdAsync<ItemDetailsServiceModel>(id);
            var highestBid = await this.bidService.GetHighestBidAmountForGivenItemAsync(id);

            var viewModel = Mapper.Map<BidDetailsViewModel>(serviceModel);
            viewModel.UserId = await this.userService.GetUserIdByUsernameAsync(this.User.Identity.Name);
            viewModel.ReturnUrl = this.HttpContext.Request.Path.ToString();
            viewModel.HighestBid = highestBid ?? 0;

            return this.View();
        }
    }
}
