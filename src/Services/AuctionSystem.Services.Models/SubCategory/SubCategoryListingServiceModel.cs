namespace AuctionSystem.Services.Models.SubCategory
{
    using Category;

    public class SubCategoryListingServiceModel : BaseSubCategoryServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public CategoryListingServiceModel Category { get; set; }
    }
}