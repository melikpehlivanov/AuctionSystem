namespace Application.Items.Commands.UpdateItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;

        public UpdateItemCommandHandler(IAuctionSystemDbContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await this.context
                .Items
                .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item == null
                || item.UserId != this.currentUserService.UserId)
            {
                throw new NotFoundException(nameof(item));
            }

            if (!await this.context
                .SubCategories
                .AnyAsync(c => c.Id == request.SubCategoryId, cancellationToken))
            {
                throw new BadRequestException("Subcategory does not exist!");
            }

            item.Title = request.Title;
            item.Description = request.Description;
            item.StartingPrice = request.StartingPrice;
            item.MinIncrease = request.MinIncrease;
            item.StartTime = request.StartTime.ToUniversalTime();
            item.EndTime = request.EndTime.ToUniversalTime();
            item.SubCategoryId = request.SubCategoryId;

            this.context.Items.Update(item);
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}