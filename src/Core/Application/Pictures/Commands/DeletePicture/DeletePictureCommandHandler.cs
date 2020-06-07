namespace Application.Pictures.Commands.DeletePicture
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AppSettingsModels;
    using CloudinaryDotNet;
    using Common.Exceptions;
    using Common.Interfaces;
    using CreatePicture;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Notifications.Models;

    public class DeletePictureCommandHandler : IRequestHandler<DeletePictureCommand>,
        INotificationHandler<ItemDeletedNotification>
    {
        private readonly Cloudinary cloudinary;
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMediator mediator;
        private readonly CloudinaryOptions options;

        public DeletePictureCommandHandler(
            IAuctionSystemDbContext context,
            ICurrentUserService currentUserService,
            IMediator mediator,
            IOptions<CloudinaryOptions> options)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mediator = mediator;
            this.options = options.Value;

            var account = new Account(
                this.options.CloudName,
                this.options.ApiKey,
                this.options.ApiSecret);

            this.cloudinary = new Cloudinary(account);
        }

        public async Task Handle(ItemDeletedNotification notification, CancellationToken cancellationToken)
        {
            await this.cloudinary.DeleteResourcesByPrefixAsync($"{notification.ItemId}/");
            await this.cloudinary.DeleteFolderAsync($"{notification.ItemId}");
        }

        public async Task<Unit> Handle(DeletePictureCommand request, CancellationToken cancellationToken)
        {
            var pictureToRemove = await this.context
                .Pictures
                .Include(p => p.Item)
                .Where(p => p.Id == request.PictureId)
                .SingleOrDefaultAsync(cancellationToken);

            if (
                pictureToRemove == null
                || pictureToRemove.Item.UserId != this.currentUserService.UserId
                || pictureToRemove.ItemId != request.ItemId)
            {
                throw new NotFoundException(nameof(Picture));
            }

            await this.cloudinary.DeleteResourcesByPrefixAsync($"{request.ItemId}/{request.PictureId}");
            this.context.Pictures.Remove(pictureToRemove);
            await this.context.SaveChangesAsync(cancellationToken);

            var pictures = await this.context
                .Pictures
                .Where(p => p.ItemId == request.ItemId)
                .AnyAsync(cancellationToken);

            if (!pictures)
            {
                await AddDefaultPicture(request.ItemId);
            }

            return Unit.Value;
        }

        private async Task AddDefaultPicture(Guid itemId)
            => await this.mediator.Send(new CreatePictureCommand {ItemId = itemId});
    }
}