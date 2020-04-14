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
            request.PageSize = request.PageSize > AppConstants.PageSize ? AppConstants.PageSize : request.PageSize;
            var skipCount = (request.PageNumber - 1) * request.PageSize;
            var items = await this.context
                .Items
                .ProjectTo<ListItemsResponseModel>(this.mapper.ConfigurationProvider)
                .Skip(skipCount)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken: cancellationToken);

            var result = PaginationHelpers.CreatePaginatedResponse(this.uriService, request, items);
            return result;
        }
    }
}
