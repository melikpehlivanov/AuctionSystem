namespace Application.Items.Queries.List
{
    using Common.Models;
    using MediatR;

    public class ListItemsQuery : PaginationQuery, IRequest<PagedResponse<ListItemsResponseModel>>
    {
    }
}
