namespace AuctionSystem.Web.SignalRHubs
{
    using System;
    using System.Threading.Tasks;
    using Application.Bids.Commands.CreateBid;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    public class BidHub : Hub
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserService currentUserService;

        public BidHub(IMediator mediator, ICurrentUserService currentUserService)
        {
            this.mediator = mediator;
            this.currentUserService = currentUserService;
        }

        public async Task Setup(string consoleId)
        {
            if (consoleId == null)
            {
                return;
            }

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, consoleId);
        }

        [Authorize]
        public async Task CreateBidAsync(string bidAmount, Guid itemId)
        {
            if (bidAmount == null || itemId == Guid.Empty)
            {
                return;
            }

            var isParsed = decimal.TryParse(bidAmount, out var parsedBidAmount);

            if (!isParsed)
            {
                return;
            }

            try
            {
                var userId = this.currentUserService.UserId;
                await this.mediator.Send(new CreateBidCommand
                {
                    Amount = parsedBidAmount,
                    ItemId = itemId,
                    UserId = userId
                });

                await this.Clients.Groups(itemId.ToString()).SendAsync("ReceivedMessage", parsedBidAmount, userId);
            }
            catch (NotFoundException)
            {
            }
            catch (BadRequestException)
            {
            }
        }
    }
}
