namespace Application.Categories.Queries.List
{
    using Common.Models;
    using MediatR;

    public class ListCategoriesQuery : IRequest<Response<ListCategoriesResponseModel>>
    {
    }
}
