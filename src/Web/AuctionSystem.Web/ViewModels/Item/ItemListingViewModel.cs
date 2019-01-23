namespace AuctionSystem.Web.ViewModels.Item
{
    using Infrastructure.Collections;

    public class ItemListingViewModel
    {
        public string Query { get; set; }
        public PaginatedList<ItemListingDto> Items { get; set; }
    }
}
