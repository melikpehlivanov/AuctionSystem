namespace Api.Models
{
    using System;
    using Application.Items.Queries.List;
    using global::Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class ItemsFilter : IMapWith<ListAllItemsQueryFilter>
    {
        [FromQuery(Name = "title")]
        public string Title { get; set; }

        [FromQuery(Name = "userId")]
        public string UserId { get; set; }

        [FromQuery(Name = "startingPrice")]
        public decimal? StartingPrice { get; set; }

        [FromQuery(Name = "startTime")]
        public DateTime? StartTime { get; set; }

        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }

        [FromQuery(Name = "minimumPicturesCount")]
        public int? MinimumPicturesCount { get; set; }
    }
}
