namespace Application.Pictures.Commands.DeletePicture
{
    using System.Threading;
    using System.Threading.Tasks;
    using AppSettingsModels;
    using CloudinaryDotNet;
    using MediatR;
    using Microsoft.Extensions.Options;
    using Notifications.Models;

    public class DeletePictureCommandHandler : IRequestHandler<DeletePictureCommand>, INotificationHandler<ItemDeletedNotification>
    {
        private readonly CloudinaryOptions options;
        private readonly Cloudinary cloudinary;

        public DeletePictureCommandHandler(IOptions<CloudinaryOptions> options, Cloudinary cloudinary)
        {
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
            => throw new System.NotImplementedException();
    }
}
