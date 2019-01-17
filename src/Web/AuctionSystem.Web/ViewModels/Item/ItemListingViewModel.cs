namespace AuctionSystem.Web.ViewModels.Item
{
    using Infrastructure.Collections;
    using Services.Models.Item;

    public class ItemListingViewModel
    {
        public PaginatedList<ItemListingServiceModel> Items { get; set; }
    }
}
