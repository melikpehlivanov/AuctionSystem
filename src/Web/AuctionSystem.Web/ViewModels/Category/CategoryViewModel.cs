namespace AuctionSystem.Web.ViewModels.Category
{
    using System.Collections.Generic;
    using Common.AutoMapping.Interfaces;
    using Services.Models.Category;

    public class CategoryViewModel : IMapWith<CategoryListingServiceModel>
    {
        public string Name { get; set; }

        public IEnumerable<SubCategoryViewModel> SubCategories { get; set; }
    }
}
