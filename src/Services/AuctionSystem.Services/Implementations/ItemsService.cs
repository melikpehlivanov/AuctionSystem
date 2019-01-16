namespace AuctionSystem.Services.Implementations
{
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using AutoMapper;
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

        public async Task<string> CreateAsync(ItemCreateServiceModel serviceModel)
        {
            if (!this.IsEntityStateValid(serviceModel))
            {
                return null;
            }

            var user = await this.Context.Users.SingleOrDefaultAsync(u => u.UserName == serviceModel.UserName);

            if (user == null ||
                !await this.Context.SubCategories.AnyAsync(c => c.Id == serviceModel.SubCategoryId))
            {
                return null;
            }

            var item = Mapper.Map<Item>(serviceModel);

            item.UserId = user.Id;

            await this.Context.AddAsync(item);

            await this.Context.SaveChangesAsync();

            return item.Id;
        }
    }
}