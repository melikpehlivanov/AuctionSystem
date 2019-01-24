namespace AuctionSystem.Services.Models.Item
{
    using System.Collections.Generic;
    using Picture;

    public class LiveItemServiceModel : BaseItemServiceModel
    {
        public string Title { get; set; }

        public ICollection<PictureDisplayServiceModel> Pictures { get; set; }
    }
}
