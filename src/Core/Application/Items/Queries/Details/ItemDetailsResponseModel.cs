namespace Application.Items.Queries.Details
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;
    using Pictures;

    public class ItemDetailsResponseModel : IMapWith<Item>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public ICollection<PictureResponseModel> Pictures { get; set; }

        public Guid SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }
    }
}