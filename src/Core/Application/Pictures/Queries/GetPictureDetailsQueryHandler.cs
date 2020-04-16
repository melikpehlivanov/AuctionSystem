namespace Application.Pictures.Queries
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetPictureDetailsQueryHandler : IRequestHandler<GetPictureDetailsQuery, Response<PictureDetailsResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public GetPictureDetailsQueryHandler(IAuctionSystemDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Response<PictureDetailsResponseModel>> Handle(GetPictureDetailsQuery request, CancellationToken cancellationToken)
        {
            var picture = await this.context
                .Pictures
                .Where(p => p.Id == request.Id)
                .ProjectTo<PictureDetailsResponseModel>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (picture == null)
            {
                throw new NotFoundException(nameof(Picture));
            }

            return new Response<PictureDetailsResponseModel>(picture);
        }
    }
}