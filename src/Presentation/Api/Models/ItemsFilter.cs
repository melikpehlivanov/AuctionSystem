namespace Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Application.Items.Queries.List;
    using global::Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class ItemsFilter : IMapWith<ListAllItemsQueryFilter>
    {
        private const string DecimalMaxValue = "79228162514264337593543950335";

        [FromQuery(Name = "title")]
        public string Title { get; set; }

        [FromQuery(Name = "userId")]
        public string UserId { get; set; }

        [FromQuery(Name = "startingPrice")]
        [Range(typeof(decimal), "0.01", DecimalMaxValue)]
        public decimal? StartingPrice { get; set; }

        [FromQuery(Name = "getLiveItems")]
        public bool? GetLiveItems { get; set; }

        [FromQuery(Name = "startTime")]
        public DateTime? StartTime { get; set; }

        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }

        [FromQuery(Name = "minimumPicturesCount")]
        [Range(1, int.MaxValue)]
        public int? MinimumPicturesCount { get; set; }

        [FromQuery(Name = "subCategoryId")]
        public Guid SubCategoryId { get; set; }
    }
}