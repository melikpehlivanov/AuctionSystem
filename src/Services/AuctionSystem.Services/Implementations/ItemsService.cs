namespace AuctionSystem.Services.Implementations
{
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.Item;

    public class ItemsService : BaseService, IItemsService
    {
        public ItemsService(AuctionSystemDbContext context) : base(context)
        {
        }

        public async Task<T> GetByIdAsync<T>(string id)
            where T : BaseItemServiceModel
        {
            if (id == null)
            {
                return null;
            }

            var item = await this.Context.Items
                .ProjectTo<T>()
                .SingleOrDefaultAsync(i => i.Id == id);

            return item;
        }
    }
}