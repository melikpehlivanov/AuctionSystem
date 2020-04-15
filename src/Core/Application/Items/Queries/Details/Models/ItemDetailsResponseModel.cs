namespace Application.Items.Queries.Details.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Common.AutoMapping.Interfaces;
    using Pictures;

    public class ItemDetailsResponseModel : IMapWith<ItemDetailsDto>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan RemainingTime => this.EndTime - DateTime.UtcNow;

        public string UserFullName { get; set; }

        public ICollection<PictureResponseModel> Pictures { get; set; }

        public string SubCategoryName { get; set; }

        public string PrimaryPicturePath => this.GetPrimaryPicturePath(this.Pictures);

        private string GetPrimaryPicturePath(IEnumerable<PictureResponseModel> pictures)
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
