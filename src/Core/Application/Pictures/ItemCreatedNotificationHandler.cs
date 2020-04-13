namespace Application.Pictures
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using AutoMapper;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Domain.Entities;
    using Items;
    using MediatR;
    using Microsoft.Extensions.Options;

    public class ItemCreatedNotificationHandler : INotificationHandler<ItemCreatedNotification>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly CloudinaryOptions options;
        private readonly Cloudinary cloudinary;

        public ItemCreatedNotificationHandler(IAuctionSystemDbContext context, IOptions<CloudinaryOptions> options)
        {
            this.context = context;
            this.options = options.Value;

            var account = new Account(
                this.options.CloudName,
                this.options.ApiKey,
                this.options.ApiSecret);

            this.cloudinary = new Cloudinary(account);
        }

        public ItemCreatedNotificationHandler(IAuctionSystemDbContext context)
        {
            this.context = context;
        }

        public async Task Handle(ItemCreatedNotification notification, CancellationToken cancellationToken)
        {
            if (!notification.Pictures.Any())
            {
                return;
            }

            var uploadResults = new ConcurrentBag<ImageUploadResult>();
            foreach (var picture in notification.Pictures)
            {
                var guid = Guid.NewGuid().ToString();
                var uploadParams = new ImageUploadParams
                {
                    PublicId = Guid.NewGuid().ToString(),
                    File = new FileDescription(guid, picture.OpenReadStream()),
                    Folder = $"{notification.ItemId}",
                };
                var uploadResult = await this.cloudinary.UploadAsync(uploadParams);
                uploadResults.Add(uploadResult);
            }

            var picturesToAdd = uploadResults.Select(picture => new Picture
            {
                Id = Guid.Parse(picture.PublicId.Substring(picture.PublicId.LastIndexOf('/') + 1)),
                ItemId = notification.ItemId,
                Url = picture.SecureUri.AbsoluteUri
            }).ToList();

            await this.context.Pictures.AddRangeAsync(picturesToAdd, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
