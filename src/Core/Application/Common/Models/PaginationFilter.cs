namespace Application.Common.Models
{
    using Admin.Queries.List;
    using global::Common.AutoMapping.Interfaces;
    using Items.Queries.List;

    public class PaginationFilter : IMapWith<ListItemsQuery>, IMapWith<ListAllUsersQuery>
    {
        private const int DefaultPageNumber = 1;

        public PaginationFilter()
        {
            this.PageNumber = DefaultPageNumber;
            this.PageSize = AppConstants.PageSize;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < DefaultPageNumber ? DefaultPageNumber : pageNumber;
            this.PageSize = pageSize >= AppConstants.PageSize || pageSize < 1 ? AppConstants.PageSize : pageSize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}