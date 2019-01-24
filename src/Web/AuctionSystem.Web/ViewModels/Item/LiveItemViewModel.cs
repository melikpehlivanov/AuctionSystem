namespace AuctionSystem.Web.ViewModels.Item
{
    using System.Collections.Generic;
    using Picture;

    public class LiveItemViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Url => $"/bid/{this.Id}";

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }
    }
}
