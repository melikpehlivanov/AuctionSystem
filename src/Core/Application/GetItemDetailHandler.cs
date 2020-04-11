namespace Application
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetItemDetailHandler : IRequestHandler<GetItemDetailQuery, ItemDetailAppModel>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public GetItemDetailHandler(IAuctionSystemDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ItemDetailAppModel> Handle(GetItemDetailQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                return null;
            }

            var item = await this.context.Items
                .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);

            var newItem = this.mapper.Map<ItemDetailAppModel>(item);
            return newItem;
        }
    }
}
