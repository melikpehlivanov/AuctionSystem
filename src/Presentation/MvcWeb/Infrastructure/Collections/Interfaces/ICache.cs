namespace MvcWeb.Infrastructure.Collections.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ViewModels.Category;

    public interface ICache
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesWithSubcategoriesAsync();
    }
}
