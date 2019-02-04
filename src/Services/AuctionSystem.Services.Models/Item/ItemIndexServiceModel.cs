namespace AuctionSystem.Services.Models.Item
{
    using System.Collections.Generic;
    using Picture;

    public class ItemIndexServiceModel : BaseItemServiceModel
    {
        public string Title { get; set; }

        public decimal StartingPrice { get; set; }

        public string UserFullName { get; set; }

        public ICollection<PictureDisplayServiceModel> Pictures { get; set; }
    }
}
