namespace MvcWeb.ViewModels.Picture
{
    using Application.Pictures;
    using Common.AutoMapping.Interfaces;

    public class PictureDisplayViewModel : IMapWith<PictureResponseModel>
    {
        public string Id { get; set; }

        public string Url { get; set; }
    }
}