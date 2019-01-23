namespace AuctionSystem.Web.ViewModels.Item
{
    using Infrastructure.Collections;

    public class ItemListingViewModel
    {
        public PaginatedList<ItemListingDto> Items { get; set; }
    }
}
