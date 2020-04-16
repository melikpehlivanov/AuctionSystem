namespace MvcWeb.ViewModels.Category
{
    using System.Collections.Generic;
    using Application.Categories.Queries.List;
    using Common.AutoMapping.Interfaces;

    public class CategoryViewModel : IMapWith<ListCategoriesResponseModel>
    {
        public string Name { get; set; }

        public IEnumerable<SubCategoryViewModel> SubCategories { get; set; }
    }
}
