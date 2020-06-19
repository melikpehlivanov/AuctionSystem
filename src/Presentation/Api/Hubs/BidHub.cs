namespace Api.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Application.Bids.Commands.CreateBid;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task Setup(string itemId)
        {
            if (itemId == null)
            {
                return;
            }

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, itemId);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task CreateBidAsync(decimal bidAmount, string itemId)
        {
            try
            {
                var userId = this.currentUserService.UserId;
                await this.mediator.Send(new CreateBidCommand
                {
                    Amount = bidAmount,
                    ItemId = Guid.Parse(itemId),
                    UserId = userId,
                });
                
                await this.Clients.Groups(itemId).SendAsync("ReceiveMessage", bidAmount, userId);
            }
            catch (NotFoundException ex)
            {
                await this.Clients.Caller.SendAsync("handleException", ex.Message);
            }
            catch (BadRequestException ex)
            {
                await this.Clients.Caller.SendAsync("handleException", ex.Message);
            }
        }
    }
}