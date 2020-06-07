namespace Application.Pictures.Commands.UpdatePicture
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using CreatePicture;
    using DeletePicture;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class UpdatePictureCommandHandler : IRequestHandler<UpdatePictureCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMediator mediator;
        
        public UpdatePictureCommandHandler(
            IAuctionSystemDbContext context,
            ICurrentUserService currentUserService,
            IMediator mediator)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(UpdatePictureCommand request, CancellationToken cancellationToken)
        {
            var item = await this.context
                .Items
                .Select(i => new
                {
                    i.Id,
                    i.UserId
                })
                .SingleOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);
            if (item.UserId != this.currentUserService.UserId)
            {
                throw new NotFoundException(nameof(Item));
            }

            if (request.PicturesToAdd.Any())
            {
                await this.mediator.Send(new CreatePictureCommand
                    {ItemId = request.ItemId, Pictures = request.PicturesToAdd}, cancellationToken);
            }

            if (request.PicturesToRemove.Any())
            {
                foreach (var pictureToRemove in request.PicturesToRemove)
                {
                    await this.mediator.Send(new DeletePictureCommand
                        {ItemId = request.ItemId, PictureId = pictureToRemove}, cancellationToken);
                }
            }

            return Unit.Value;
        }
    }
}