namespace Application.Common.Models
{
    using global::Common.AutoMapping.Interfaces;
    using Items.Queries.List;

    public class PaginationQuery : IMapWith<ListItemsQuery>
    {
        public PaginationQuery()
        {
            this.PageNumber = 1;
            this.PageSize = AppConstants.PageSize;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize >= AppConstants.PageSize ? AppConstants.PageSize : pageSize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
