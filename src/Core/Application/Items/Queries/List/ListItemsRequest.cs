namespace Application.Items.Queries.List
{
    using Common.Models;
    using MediatR;

    public class ListItemsRequest : PaginationQuery, IRequest<PagedResponse<ListItemsResponseModel>>
    {
    }
}
