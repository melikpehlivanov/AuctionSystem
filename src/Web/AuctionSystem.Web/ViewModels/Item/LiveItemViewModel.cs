namespace AuctionSystem.Web.ViewModels.Item
{
    using System.Collections.Generic;
    using Common.AutoMapping.Interfaces;
    using Picture;
    using Services.Models.Item;

    public class LiveItemViewModel : IMapWith<LiveItemServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Url => $"/bid/{this.Id}";

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }
    }
}
