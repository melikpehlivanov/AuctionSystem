namespace AuctionSystem.Services.Models.Category
{
    using System.Collections.Generic;
    using SubCategory;

    public class CategoryListingServiceModel : BaseCategoryServiceModel
    {
        public string Name { get; set; }

        public IEnumerable<SubCategoryListingServiceModel> SubCategories { get; set; }
    }
}