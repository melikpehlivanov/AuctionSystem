namespace AuctionSystem.Web.SignalRHubs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Services.Interfaces;
    using Services.Models.Bid;
    using Services.Models.Item;

    public class BidHub : Hub
    {
        private readonly IBidService bidService;
        private readonly IItemsService itemsService;

        public BidHub(IBidService bidService, IItemsService itemsService)
        {
            this.bidService = bidService;
            this.itemsService = itemsService;
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
        public async Task CreateBidAsync(decimal bidAmount, string consoleId)
        {
            if (itemId == null)
            {
                return;
            }

            try
            {
                var userId = this.currentUserService.UserId;
                await this.mediator.Send(new CreateBidCommand
                {
                    Amount = bidAmount,
                    ItemId = Guid.Parse(itemId),
                    UserId = userId,
                });

                await this.Clients.Groups(itemId).SendAsync("ReceivedMessage", bidAmount, userId);
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