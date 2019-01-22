namespace AuctionSystem.Web.Areas.Bid.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.AutoMapping.Interfaces;
    using Services.Models.Item;
    using ViewModels.Picture;

    public class BidDetailsViewModel : IMapWith<ItemDetailsServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan RemainingTime => this.EndTime - DateTime.UtcNow;

        public string UserId { get; set; }

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }

        public string ReturnUrl { get; set; }

        public decimal HighestBid { get; set; }

        public ICollection<string> CurrentHighestPriceDigits =>
            this.HighestBid.ToString("F2").Select(s => Convert.ToString(s)).ToList();
    }
}
