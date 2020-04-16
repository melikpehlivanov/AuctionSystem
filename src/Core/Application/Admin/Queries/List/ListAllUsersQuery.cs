namespace Application.Admin.Queries.List
{
    using Common.Models;
    using MediatR;

    public class ListAllUsersQuery : PaginationFilter, IRequest<PagedResponse<ListAllUsersResponseModel>>
    {
        public ListAllUsersQueryFilter Filters { get; set; } = null;
    }
}
