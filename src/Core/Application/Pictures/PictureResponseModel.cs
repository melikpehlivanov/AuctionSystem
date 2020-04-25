namespace Application.Pictures
{
    using System;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;

    public class PictureResponseModel : IMapWith<Picture>
    {
        public Guid Id { get; set; }

        public string Url { get; set; }
    }
}