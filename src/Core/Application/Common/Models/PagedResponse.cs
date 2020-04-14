namespace Application.Common.Models
{
    using System.Collections.Generic;

    public class PagedResponse<T>
    {
        public PagedResponse()
        {
            this.PageNumber = 1;
            this.PageSize = AppConstants.PageSize;
        }

        public PagedResponse(IEnumerable<T> data)
        {
            this.Data = data;
        }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public string NextPage { get; set; }

        public string PreviousPage { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}
