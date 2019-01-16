namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.SubCategory;

    public interface ICategoriesService
    {
        Task<IEnumerable<T>> GetAllSubCategoriesAsync<T>()
            where T : BaseSubCategoryServiceModel;
    }
}