namespace Application.Items.Queries.List
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Helpers;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using global::Common;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class ListItemsQueryHandler : IRequestHandler<ListItemsQuery, PagedResponse<ListItemsResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly IDateTime dateTime;

        public ListItemsQueryHandler(IAuctionSystemDbContext context, IMapper mapper, IDateTime dateTime)
        {
            this.context = context;
            this.mapper = mapper;
            this.dateTime = dateTime;
        }

        public async Task<PagedResponse<ListItemsResponseModel>> Handle(
            ListItemsQuery request,
            CancellationToken cancellationToken)
        {
            var skipCount = (request.PageNumber - 1) * request.PageSize;
            var queryable = this.context
                .Items
                .Include(i => i.Pictures)
                .Include(u => u.User)
                .AsQueryable();

            var totalItemsCount = await this.context.Items.CountAsync(cancellationToken);
            if (request?.Filters == null)
            {
                return PaginationHelper.CreatePaginatedResponse(request, await queryable
                    .Skip(skipCount)
                    .Take(request.PageSize)
                    .ProjectTo<ListItemsResponseModel>(this.mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken), totalItemsCount);
            }

            queryable = this.AddFiltersOnQuery(request.Filters, queryable);
            totalItemsCount = await queryable.CountAsync(cancellationToken);
            var itemsList = await queryable
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var items = itemsList
                .Select(this.mapper.Map<ListItemsResponseModel>)
                .ToList();

            var result = PaginationHelper.CreatePaginatedResponse(request, items, totalItemsCount);
            return result;
        }

        private IQueryable<Item> AddFiltersOnQuery(ListAllItemsQueryFilter filters, IQueryable<Item> queryable)
        {
            if (!string.IsNullOrEmpty(filters?.Title))
            {
                queryable = queryable.Where(i => i.Title.ToLower().Contains(filters.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(filters?.UserId))
            {
                queryable = queryable.Where(i => i.UserId == filters.UserId);
            }

            if (filters?.GetLiveItems == true)
            {
                queryable = queryable.Where(i => i.StartTime < dateTime.UtcNow && i.EndTime > dateTime.UtcNow);
            }

            if (filters?.MinPrice != null)
            {
                queryable = queryable.Where(i => i.StartingPrice >= filters.MinPrice);
            }

            if (filters?.MaxPrice != null)
            {
                queryable = queryable.Where(i => i.StartingPrice <= filters.MaxPrice);
            }

            if (filters?.StartTime != null)
            {
                queryable = queryable.Where(i => i.StartTime >= filters.StartTime.Value.ToUniversalTime());
            }

            if (filters?.EndTime != null)
            {
                queryable = queryable.Where(i => i.EndTime <= filters.EndTime.Value.ToUniversalTime());
            }

            if (filters?.MinimumPicturesCount != null)
            {
                queryable = queryable.Where(i => i.Pictures.Count >= filters.MinimumPicturesCount);
            }

            if (filters?.SubCategoryId != Guid.Empty)
            {
                queryable = queryable.Where(i => i.SubCategoryId == filters.SubCategoryId);
            }

            return queryable;
        }
    }
}