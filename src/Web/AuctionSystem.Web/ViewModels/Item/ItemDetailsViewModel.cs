namespace AuctionSystem.Web.ViewModels.Item
{
    using System;
    using System.Collections.Generic;
    using Common.AutoMapping.Interfaces;
    using Picture;
    using Services.Models.Item;

    public class ItemDetailsViewModel : IMapWith<ItemDetailsServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan RemainingTime => this.EndTime - DateTime.UtcNow;

        public ICollection<PictureDisplayViewModel> Pictures { get; set; }
        
        public string UserUserName { get; set; }
    }
}