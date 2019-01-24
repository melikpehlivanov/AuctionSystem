namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Category;

    public interface ICategoriesService
    {
        Task<IEnumerable<T>> GetAllCategoriesWithSubCategoriesAsync<T>()
            where T : BaseCategoryServiceModel;
    }
}