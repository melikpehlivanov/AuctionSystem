namespace AuctionSystem.Services.Implementations
{
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using AutoMapper;
    using Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.Bid;

    public class BidService : BaseService, IBidService
    {
        public BidService(AuctionSystemDbContext context)
            : base(context)
        {
        }

        public async Task<bool> CreateBidAsync(BidCreateServiceModel model)
        {
            if (!this.IsEntityStateValid(model))
            {
                return false;
            }

            var bid = Mapper.Map<Bid>(model);

            try
            {
                await this.Context.Bids.AddAsync(bid);
                await this.Context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<decimal?> GetHighestBidAmountForGivenItemAsync(string id)
        {
            var highestBid = await this.Context
                .Bids
                .Select(b => new
                {
                    b.Amount,
                    b.ItemId
                })
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync(b => b.ItemId == id);

            return highestBid?.Amount;
        }
    }
}
