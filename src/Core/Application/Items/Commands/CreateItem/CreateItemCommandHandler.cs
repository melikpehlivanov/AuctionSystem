namespace Application.Items.Commands.CreateItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using AutoMapper;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Application.Pictures.Commands.CreatePicture;
    using System.Linq;

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Response<ItemResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly ICurrentUserService userService;
        private readonly IMediator mediator;

        public CreateItemCommandHandler(
            IMapper mapper,
            IAuctionSystemDbContext context,
            ICurrentUserService userService,
            IMediator mediator
            )
        {
            this.mapper = mapper;
            this.context = context;
            this.userService = userService;
            this.mediator = mediator;
            this.userService = userService;
        }

        public async Task<Response<ItemResponseModel>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            if (this.userService.UserId == null ||
                !await this.context.SubCategories.AnyAsync(c => c.Id == request.SubCategoryId, cancellationToken))
            {
                throw new BadRequestException("An error occured while creating item.");
            }

            var item = this.mapper.Map<Item>(request);
            item.UserId = this.userService.UserId;

            await this.context.Items.AddAsync(item, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);

            if (request.Pictures.Any())
            {
                await this.mediator.Send(new CreatePictureCommand { ItemId = item.Id, Pictures = request.Pictures }, cancellationToken);
            }

            return new Response<ItemResponseModel>(new ItemResponseModel(item.Id));
        }
    }
}
