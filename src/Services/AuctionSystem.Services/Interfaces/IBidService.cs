namespace AuctionSystem.Services.Interfaces
{
    using System.Threading.Tasks;
    using Models.Bid;

    public interface IBidService
    {
        Task<bool> CreateBidAsync(BidCreateServiceModel model);

        Task<decimal?> GetHighestBidAmountForGivenItemAsync(string id);
    }
}
