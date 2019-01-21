namespace AuctionSystem.Web.SignalRHubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class BidHub : Hub
    {
        public async Task Setup(string consoleId)
        {
            if (consoleId == null)
            {
                return;
            }

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, consoleId);
        }
    }
}
