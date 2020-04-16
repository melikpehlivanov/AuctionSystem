namespace Application.Items.Queries.Details
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetItemDetailsQueryHandler : IRequestHandler<GetItemDetailsQuery, Response<ItemDetailsResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public GetItemDetailsQueryHandler(IAuctionSystemDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Response<ItemDetailsResponseModel>> Handle(GetItemDetailsQuery request, CancellationToken cancellationToken)
        {
            var item = await this.context
                .Items
                .ProjectTo<ItemDetailsResponseModel>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item == null)
            {
                throw new NotFoundException(nameof(Item));
            }

            return new Response<ItemDetailsResponseModel>(item);
        }
    }
}
