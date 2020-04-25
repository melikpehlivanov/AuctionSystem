namespace MvcWeb.ViewModels.Item
{
    using Infrastructure.Collections;

    public class ItemSearchViewModel
    {
        public string Query { get; set; }

        public PaginatedList<ItemListingDto> Items { get; set; }
    }
}
