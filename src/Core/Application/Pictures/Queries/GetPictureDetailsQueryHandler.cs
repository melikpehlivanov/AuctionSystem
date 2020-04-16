namespace Application.Pictures.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetPictureDetailsQueryHandler : IRequestHandler<GetPictureDetailsQuery, MultiResponse<PictureDetailsResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public GetPictureDetailsQueryHandler(IAuctionSystemDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<MultiResponse<PictureDetailsResponseModel>> Handle(GetPictureDetailsQuery request, CancellationToken cancellationToken)
        {
            var picture = await this.context
                .Pictures
                .ProjectTo<PictureDetailsResponseModel>(this.mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);

            if (picture == null)
            {
                throw new NotFoundException(nameof(Picture));
            }

            return new MultiResponse<PictureDetailsResponseModel>(picture);
        }
    }
}
