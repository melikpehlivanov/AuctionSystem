namespace Application.Items.Queries.Details
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetItemDetailsQueryHandler : IRequestHandler<GetItemDetailsRequest, ItemDetailsResponseModel>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public GetItemDetailsQueryHandler(IAuctionSystemDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ItemDetailsResponseModel> Handle(GetItemDetailsRequest request, CancellationToken cancellationToken)
        {
            var item = await this.context
                .Items
                .ProjectTo<ItemDetailsDto>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item == null)
            {
                throw new NotFoundException($"Item with Id {request.Id} does not exist!");
            }

            var result = this.mapper.Map<ItemDetailsResponseModel>(item);
            return result;
        }
    }
}
