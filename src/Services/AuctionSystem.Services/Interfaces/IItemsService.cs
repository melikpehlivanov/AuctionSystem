namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Item;

    public interface IItemsService
    {
        Task<T> GetByIdAsync<T>(string id)
            where T : BaseItemServiceModel;

        Task<IEnumerable<T>> GetHottestItemsAsync<T>()
            where T : BaseItemServiceModel;

        Task<IEnumerable<T>> GetAllLiveItemsAsync<T>()
            where T : BaseItemServiceModel;

        Task<string> CreateAsync(ItemCreateServiceModel serviceModel);

        Task<IEnumerable<T>> GetAllItemsInGivenCategoryByCategoryIdAsync<T>(string id)
            where T : BaseItemServiceModel;

        Task<IEnumerable<T>> GetAllItemsAsync<T>();

        Task<IEnumerable<T>> SearchByTitleAsync<T>(string query)
            where T : BaseItemServiceModel;

        Task<bool> DeleteAsync(string id);
    }
}