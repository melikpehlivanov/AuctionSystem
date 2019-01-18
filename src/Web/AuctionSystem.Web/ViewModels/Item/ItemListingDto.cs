namespace AuctionSystem.Web.ViewModels.Item
{
    using System.Collections.Generic;
    using Common.AutoMapping.Interfaces;
    using Picture;
    using Services.Models.Item;

    public class ItemListingDto : IMapWith<ItemListingServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal StartingPrice { get; set; }

        public string UserFullName { get; set; }

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }
    }
}
