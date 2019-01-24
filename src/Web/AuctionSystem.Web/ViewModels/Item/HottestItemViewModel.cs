namespace AuctionSystem.Web.ViewModels.Item
{
    using System.Collections.Generic;
    using Common;
    using Picture;

    public class HottestItemViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal StartingPrice { get; set; }

        public string Url => $"/items/details/{this.Id}/{this.Title.GenerateSlug()}";

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }
    }
}
