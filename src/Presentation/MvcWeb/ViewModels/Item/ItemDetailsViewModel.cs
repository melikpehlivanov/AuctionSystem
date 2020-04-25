namespace MvcWeb.ViewModels.Item
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application;
    using Application.Items.Queries.Details;
    using Common.AutoMapping.Interfaces;
    using Picture;

    public class ItemDetailsViewModel : IMapWith<ItemDetailsResponseModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan RemainingTime { get; set; }

        public string UserId { get; set; }

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }

        public string SubCategoryName { get; set; }

        public string PrimaryPicturePath => this.GetPrimaryPicturePath(this.Pictures);

        private string GetPrimaryPicturePath(IEnumerable<PictureDisplayViewModel> pictures)
        {
            if (!pictures.Any())
            {
                return AppConstants.DefaultPictureUrl;
            }
            var firstPic = pictures.First();

            return firstPic?.Url;
        }
    }
}