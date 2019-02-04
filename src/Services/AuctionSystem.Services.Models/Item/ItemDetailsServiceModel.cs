namespace AuctionSystem.Services.Models.Item
{
    using System;
    using System.Collections.Generic;
    using AuctionUser;
    using Picture;

    public class ItemDetailsServiceModel : BaseItemServiceModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ICollection<PictureDisplayServiceModel> Pictures { get; set; }
        
        public AuctionUserDetailsServiceModel User { get; set; }
    }
}