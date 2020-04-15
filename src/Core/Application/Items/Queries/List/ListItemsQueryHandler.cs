namespace Application.Items.Queries.List
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Helpers;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class ListItemsQueryHandler : IRequestHandler<ListItemsQuery, PagedResponse<ListItemsResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly IUriService uriService;

        public ListItemsQueryHandler(IAuctionSystemDbContext context, IMapper mapper, IUriService uriService)
        {
            this.context = context;
            this.mapper = mapper;
            this.uriService = uriService;
        }

        public async Task<PagedResponse<ListItemsResponseModel>> Handle(
            ListItemsQuery request,
            CancellationToken cancellationToken)
        {
            var skipCount = (request.PageNumber - 1) * request.PageSize;
            var queryable = this.context.Items.AsQueryable();

            queryable = AddFiltersOnQuery(request.Filters, queryable);
            var items = await queryable
                .Skip(skipCount)
                .Take(request.PageSize)
                .ProjectTo<ListItemsResponseModel>(this.mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = PaginationHelpers.CreatePaginatedResponse(this.uriService, request, items);
            return result;
        }

        private static IQueryable<Item> AddFiltersOnQuery(ListAllItemsQueryFilter filters, IQueryable<Item> queryable)
        {
            if (!string.IsNullOrEmpty(filters?.Title))
            {
                queryable = queryable.Where(i => i.Title.Contains(filters.Title));
            }

            if (!string.IsNullOrEmpty(filters?.UserId))
            {
                queryable = queryable.Where(i => i.UserId == filters.UserId);
            }

            if (filters?.StartingPrice != null)
            {
                queryable = queryable.Where(i => i.StartingPrice >= filters.StartingPrice);
            }

            if (filters?.StartTime != null)
            {
                queryable = queryable.Where(i => i.StartTime >= filters.StartTime);
            }

            if (filters?.EndTime != null)
            {
                queryable = queryable.Where(i => i.EndTime <= filters.EndTime);
            }

            if (filters?.MinimumPicturesCount != null)
            {
                queryable = queryable.Where(i => i.Pictures.Count >= filters.MinimumPicturesCount);
            }

            return queryable;
        }
    }
}
