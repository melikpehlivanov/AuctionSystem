namespace Application.Pictures.Commands.DeletePicture
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using AppSettingsModels;
    using CloudinaryDotNet;
    using Domain.Entities;
    using MediatR;
    using Microsoft.Extensions.Options;
    using Notifications.Models;
    using Common.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class DeletePictureCommandHandler : IRequestHandler<DeletePictureCommand>, INotificationHandler<ItemDeletedNotification>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly CloudinaryOptions options;
        private readonly Cloudinary cloudinary;

        public DeletePictureCommandHandler(
            IAuctionSystemDbContext context, 
            ICurrentUserService currentUserService, 
            IOptions<CloudinaryOptions> options)
        {
            this.context = context;
            this.currentUserService = currentUserService;
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
                .FindAsync(request.PictureId);

            if (pictureToRemove == null || pictureToRemove.CreatedBy != this.currentUserService.UserId)
            {
                throw new NotFoundException(nameof(Picture), request.PictureId);
            }

            await this.cloudinary.DeleteResourcesByPrefixAsync($"{request.ItemId}/{request.PictureId}");
            this.context.Pictures.Remove(pictureToRemove);
            await this.context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
