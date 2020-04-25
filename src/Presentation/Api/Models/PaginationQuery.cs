namespace Api.Models
{
    using System.ComponentModel.DataAnnotations;
    using Application;
    using Application.Common.Models;
    using global::Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class PaginationQuery : IMapWith<PaginationFilter>
    {
        [FromQuery(Name = "pageNumber")]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}