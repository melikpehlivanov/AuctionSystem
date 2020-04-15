﻿namespace Application.Items.Queries.Details.Models
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;
    using Pictures;

    public class ItemDetailsDto : IMapWith<Item>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ICollection<PictureResponseModel> Pictures { get; set; }

        public string UserFullName { get; set; }

        public string SubCategoryName { get; set; }
    }
}