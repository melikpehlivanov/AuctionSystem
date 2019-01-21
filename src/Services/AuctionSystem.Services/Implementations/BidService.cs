namespace AuctionSystem.Services.Implementations
{
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using AutoMapper;
    using Data;
    using Interfaces;
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
    }
}
