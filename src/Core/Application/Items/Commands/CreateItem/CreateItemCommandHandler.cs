namespace Application.Items.Commands.CreateItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Pictures.Commands.CreatePicture;

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Response<ItemResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ICurrentUserService userService;

        public CreateItemCommandHandler(IAuctionSystemDbContext context,
            IMapper mapper,
            IMediator mediator,
            ICurrentUserService userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.mediator = mediator;
            this.userService = userService;
        }


        public async Task<Response<ItemResponseModel>> Handle(CreateItemCommand request,
            CancellationToken cancellationToken)
        {
            if (this.userService.UserId == null
                || !await this.context.SubCategories.AnyAsync(c => c.Id == request.SubCategoryId, cancellationToken))
            {
                throw new BadRequestException(ExceptionMessages.Item.CreateItemErrorMessage);
            }

            var item = this.mapper.Map<Item>(request);
            item.UserId = this.userService.UserId;
            item.StartTime = item.StartTime.ToUniversalTime();
            item.EndTime = item.EndTime.ToUniversalTime();

            await this.context.Items.AddAsync(item, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);

            await this.mediator.Send(new CreatePictureCommand { ItemId = item.Id, Pictures = request.Pictures },
                cancellationToken);

            return new Response<ItemResponseModel>(new ItemResponseModel(item.Id));
        }
    }
}