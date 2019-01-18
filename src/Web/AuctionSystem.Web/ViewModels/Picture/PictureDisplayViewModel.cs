namespace AuctionSystem.Web.ViewModels.Picture
{
    using Common.AutoMapping.Interfaces;
    using Services.Models.Picture;

    public class PictureDisplayViewModel : IMapWith<PictureDisplayServiceModel>
    {
        public string Id { get; set; }

        public string Url { get; set; }
    }
}