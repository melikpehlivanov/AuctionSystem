namespace AuctionSystem.Web.ViewModels.Picture
{
    using Application.Pictures;
    using global::Common.AutoMapping.Interfaces;

    public class PictureDisplayViewModel : IMapWith<PictureResponseModel>
    {
        public string Id { get; set; }

        public string Url { get; set; }
    }
}