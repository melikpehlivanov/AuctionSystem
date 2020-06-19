namespace Application.Bids.Queries.Details
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Interfaces;
    using Common.Models;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetHighestBidDetailsQueryHandler : IRequestHandler<GetHighestBidDetailsQuery,
        Response<GetHighestBidDetailsResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public GetHighestBidDetailsQueryHandler(IAuctionSystemDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Response<GetHighestBidDetailsResponseModel>> Handle(GetHighestBidDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var bid = await this.context
                .Bids
                .Where(b => b.ItemId == request.ItemId)
                .OrderByDescending(b => b.Amount)
                .ProjectTo<GetHighestBidDetailsResponseModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return new Response<GetHighestBidDetailsResponseModel>(bid);
        }
    }
}