namespace AuctionSystem.Web.ViewModels.Item
{
    using System.Collections.Generic;
    using Application.Items.Queries.List;
    using global::Common.AutoMapping.Interfaces;
    using Picture;

    public class LiveItemViewModel : IMapWith<ListItemsResponseModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Url => $"/bid/{this.Id}";

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }
    }
}
