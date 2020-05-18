namespace Application.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class PagedResponse<T>
    {
        public PagedResponse()
        {
            this.PageNumber = 1;
            this.PageSize = AppConstants.PageSize;
        }

        public PagedResponse(IEnumerable<T> data, int totalDataCountInDatabase)
        {
            this.Data = data;
            this.TotalPages = (int)Math.Ceiling(totalDataCountInDatabase / (double)AppConstants.PageSize);
        }

        public int TotalPages { get; set; }

        public int PageNumber { get; set; }

        public int? PageSize { get; set; }

        public int? NextPage { get; set; }

        public int? PreviousPage { get; set; }

        public IEnumerable<T> Data { get; set; }
        
        public int TotalDataCount { get; set; }
    }
}