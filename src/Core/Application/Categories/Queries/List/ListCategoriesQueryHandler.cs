namespace Application.Categories.Queries.List
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using AutoMapper;
    using Common.Models;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using System.Linq;
    using Application.Common.Exceptions;

    public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, Response<ListCategoriesResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public ListCategoriesQueryHandler(IAuctionSystemDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Response<ListCategoriesResponseModel>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await this.context
                .Categories
                .Include(c => c.SubCategories)
                .ProjectTo<ListCategoriesResponseModel>(this.mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);

            if (!categories.Any())
            {
                throw new NotFoundException("Currently there are not any categories.");
            }

            return new Response<ListCategoriesResponseModel>(categories);
        }
    }
}
