namespace AuctionSystem.Web.ViewModels.Item
{
    using System.Collections.Generic;
    using Application.Items.Queries.List;
    using Common;
    using global::Common.AutoMapping.Interfaces;
    using Picture;

    public class HottestItemViewModel : IMapWith<ListItemsResponseModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal StartingPrice { get; set; }

        public string Url => $"/items/details/{this.Id}/{this.Title.GenerateSlug()}";

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }
    }
}
