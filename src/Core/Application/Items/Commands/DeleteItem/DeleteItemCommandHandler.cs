namespace Application.Items.Commands.DeleteItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using MediatR;

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;

        public DeleteItemCommandHandler(IAuctionSystemDbContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await this.context
                .Items
                .FindAsync(request.Id);

            if (itemToDelete == null || itemToDelete.UserId != this.currentUserService.UserId)
            {
                throw new NotFoundException(nameof(Item), request.Id);
            }

            this.context.Items.Remove(itemToDelete);
            await this.context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
