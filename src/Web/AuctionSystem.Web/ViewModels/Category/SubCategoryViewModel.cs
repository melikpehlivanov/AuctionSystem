namespace AuctionSystem.Web.ViewModels.Category
{
    using Common.AutoMapping.Interfaces;
    using Services.Models.SubCategory;

    public class SubCategoryViewModel : IMapWith<SubCategoryListingServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
