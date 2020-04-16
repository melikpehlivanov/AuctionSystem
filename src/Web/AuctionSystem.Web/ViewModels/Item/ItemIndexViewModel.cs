namespace AuctionSystem.Web.ViewModels.Item
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Items.Queries.List;
    using Common;
    using global::Common.AutoMapping.Interfaces;
    using Picture;

    public class ItemIndexViewModel : IMapWith<ListItemsResponseModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal StartingPrice { get; set; }

        public string UserFullName { get; set; }

        public string DetailsUrl => $"/items/details/{this.Id}/{this.Title.GenerateSlug()}";

        public string DeleteUrl => $"/items/delete/{this.Id}/{this.Title.GenerateSlug()}";

        public string EditUrl => $"/items/edit/{this.Id}/{this.Title.GenerateSlug()}";

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }

        public string PrimaryPicturePath => this.GetPrimaryPicturePath(this.Pictures);

        private string GetPrimaryPicturePath(IEnumerable<PictureDisplayViewModel> pictures)
        {
            if (!pictures.Any())
            {
                return WebConstants.DefaultPictureUrl;
            }
            var firstPic = pictures.First();

            return firstPic?.Url;
        }
    }
}
