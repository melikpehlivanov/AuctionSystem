namespace MvcWeb.Infrastructure.Collections
{
    using System.Collections;
    using System.Collections.Generic;
    using Application.Common.Models;
    using Common.AutoMapping.Interfaces;
    using Interfaces;

    public class PaginatedList<T> : IPaginatedList, IEnumerable<T>, IMapWith<PagedResponse<T>>
    {
        private readonly IEnumerable<T> data;

        public PaginatedList(IEnumerable<T> data, int pageIndex, int totalPages)
        {
            this.data = data;

            this.PageIndex = pageIndex;
            this.TotalPages = totalPages;
        }

        public IEnumerator<T> GetEnumerator() => this.data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public int PageIndex { get; }

        public int TotalPages { get; }

        public bool HasPreviousPage => this.PageIndex > 1;

        public bool HasNextPage => this.PageIndex < this.TotalPages;
    }
}
