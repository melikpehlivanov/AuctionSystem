namespace Api.Models
{
    using Application.Common.Models;
    using global::Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class PaginationQuery : IMapWith<PaginationFilter>
    {
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
    }
}
