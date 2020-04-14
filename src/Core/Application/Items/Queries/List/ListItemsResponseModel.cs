namespace Application.Items.Queries.List
{
    using System;
    using System.Collections.Generic;
    using Pictures;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;

    public class ListItemsResponseModel : IMapWith<Item>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public decimal StartingPrice { get; set; }

        public string UserFullName { get; set; }

        public ICollection<PictureResponseModel> Pictures { get; set; }
    }
}
