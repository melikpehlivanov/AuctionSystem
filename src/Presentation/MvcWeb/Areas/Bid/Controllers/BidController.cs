namespace MvcWeb.Areas.Bid.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Application.Bids.Queries.Details;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Application.Items.Queries.Details;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using MvcWeb.Controllers;

    [Area("Bid")]
    public class BidController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public BidController(IMapper mapper, ICurrentUserService currentUserService)
        {
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        [Route("/bid/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                this.ShowErrorMessage(NotificationMessages.BidNotFound);
                return this.RedirectToHome();
            }

            try
            {
                var itemDetailResponse = await this.Mediator.Send(new GetItemDetailsQuery(id));
                var highestBidForItemResponse = await this.Mediator.Send(new GetHighestBidDetailsQuery(id));
                var viewModel = this.mapper.Map<BidDetailsViewModel>(itemDetailResponse.Data);
                viewModel.UserId = this.currentUserService.UserId;
                viewModel.ReturnUrl = this.HttpContext.Request.Path.ToString();
                viewModel.HighestBid = highestBidForItemResponse?.Data?.Amount ?? 0;

                return this.View(viewModel);
            }
            catch (NotFoundException)
            {
                this.ShowErrorMessage(NotificationMessages.BidNotFound);
                return this.RedirectToHome();
            }
            catch (ValidationException)
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError); 
                return this.RedirectToHome();
            }
        }
    }
}
