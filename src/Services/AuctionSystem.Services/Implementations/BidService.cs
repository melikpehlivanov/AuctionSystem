namespace AuctionSystem.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using AutoMapper;
    using Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.Bid;
    using Models.Item;

    public class BidService : BaseService, IBidService
    {
        public BidService(IMapper mapper, AuctionSystemDbContext context) 
            : base(mapper, context)
        {
        }

        public async Task<bool> CreateBidAsync(BidCreateServiceModel model)
        {
            if (!this.IsEntityStateValid(model))
            {
                return false;
            }

            var bid = this.mapper.Map<Bid>(model);

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

        public bool CanBid(ItemDetailsServiceModel model)
        {
            return model.EndTime >= DateTime.UtcNow && model.StartTime < DateTime.UtcNow;
        }
    }
}
