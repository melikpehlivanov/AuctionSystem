namespace AuctionSystem.Services.Interfaces
{
    using System.Threading.Tasks;
    using Models.Bid;
    using Models.Item;

    public interface IBidService
    {
        Task<bool> CreateBidAsync(BidCreateServiceModel model);

        Task<decimal?> GetHighestBidAmountForGivenItemAsync(string id);

        bool CanBid(ItemDetailsServiceModel model);
    }
}
