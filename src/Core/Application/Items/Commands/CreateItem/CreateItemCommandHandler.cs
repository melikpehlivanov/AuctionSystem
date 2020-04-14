namespace Application.Items.Commands.CreateItem
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using AutoMapper;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Response<Guid>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly IUserManager userManager;
        private readonly IMediator mediator;

        public CreateItemCommandHandler(
            IMapper mapper, 
            IAuctionSystemDbContext context, 
            IUserManager userManager, 
            IMediator mediator)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.mediator = mediator;
        }

        public async Task<Response<Guid>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var userId = await this.userManager.GetUserIdByUsernameAsync(request.UserName);

            if (userId == null ||
                !await this.context.SubCategories.AnyAsync(c => c.Id == request.SubCategoryId, cancellationToken))
            {
                throw new BadRequestException("An error occured while creating item.");
            }

            var item = this.mapper.Map<Item>(request);
            item.UserId = userId;

            await this.context.Items.AddAsync(item, cancellationToken);
            await this.mediator.Publish(new ItemCreatedNotification(item.Id, request.Pictures), cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);

            return new Response<Guid>(item.Id);
        }
    }
}
