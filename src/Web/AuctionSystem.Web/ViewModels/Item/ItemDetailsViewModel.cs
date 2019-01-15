namespace AuctionSystem.Web.ViewModels.Item
{
    using Common.AutoMapping.Interfaces;
    using Services.Models.Item;

    public class ItemDetailsViewModel : IMapWith<ItemDetailsServiceModel>
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}