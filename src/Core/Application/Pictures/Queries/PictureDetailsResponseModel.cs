namespace Application.Pictures.Queries
{
    using System;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;

    public class PictureDetailsResponseModel : IMapWith<Picture>
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public Guid ItemId { get; set; }

        public string ItemUserId { get; set; }
    }
}