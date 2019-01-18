namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Item;

    public interface IItemsService
    {
        Task<T> GetByIdAsync<T>(string id)
            where T : BaseItemServiceModel;

        Task<string> CreateAsync(ItemCreateServiceModel serviceModel);

        Task<IEnumerable<T>> GetAllItemsInGivenCategoryByCategoryIdAsync<T>(string id)
            where T : BaseItemServiceModel;

        Task<IEnumerable<T>> GetAllItems<T>();
    }
}