namespace AuctionSystem.Web.ViewModels.Item
{
    using Common.AutoMapping.Interfaces;
    using Services.Models.Item;

    public class ItemListingViewModel : IMapWith<ItemListingServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal StartingPrice { get; set; }
    }
}
