namespace Application.Items.Queries.List
{
    using Common.Models;
    using MediatR;

    public class ListItemsQuery : PaginationFilter, IRequest<PagedResponse<ListItemsResponseModel>>
    {
        public ListAllItemsQueryFilter Filters { get; set; }
    }
}