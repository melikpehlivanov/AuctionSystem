namespace Application.Items.Commands.DeleteItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using Domain.Entities;
    using MediatR;
    using Notifications.Models;

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMediator mediator;

        public DeleteItemCommandHandler(IAuctionSystemDbContext context, ICurrentUserService currentUserService, IMediator mediator)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await this.context
                .Items
                .FindAsync(request.Id);

            if (itemToDelete == null
                || itemToDelete.UserId != this.currentUserService.UserId && !this.currentUserService.IsAdmin)
            {
                throw new NotFoundException(nameof(Item));
            }

            this.context.Items.Remove(itemToDelete);
            await this.context.SaveChangesAsync(cancellationToken);
            await this.mediator.Publish(new ItemDeletedNotification(itemToDelete.Id), cancellationToken);

            return Unit.Value;
        }
    }
}