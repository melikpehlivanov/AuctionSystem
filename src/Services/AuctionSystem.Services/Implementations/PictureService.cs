namespace AuctionSystem.Services.Implementations
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Models;

    public class PictureService : BaseService, IPictureService
    {
        private readonly CloudinaryOptions options;
        private readonly Cloudinary cloudinary;

        public PictureService(IMapper mapper, AuctionSystemDbContext context, IOptions<CloudinaryOptions> options)
            : base(mapper, context)
        {
            this.options = options.Value;

            var account = new Account(
                this.options.CloudName,
                this.options.ApiKey,
                this.options.ApiSecret);

            this.cloudinary = new Cloudinary(account);
        }

        public void Delete(string itemId)
            => this.cloudinary.DeleteResourcesByPrefix($"{itemId}/");

        public async Task Delete(string itemId, string pictureId)
        {
            this.cloudinary.DeleteResourcesByPrefix($"{itemId}/{pictureId}");

            var pictureToRemove = await this.Context
                .Pictures
                .FindAsync(pictureId);

            if (pictureToRemove == null)
            {
                return;
            }

            this.Context.Pictures.Remove(pictureToRemove);
            await this.Context.SaveChangesAsync();
        }

        public async Task<T> GetPictureById<T>(string pictureId)
            => await this.Context
                .Pictures
                .Where(p => p.Id == pictureId)
                .ProjectTo<T>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<UploadResult>> Upload(ICollection<Stream> pictureStreams, string itemId)
        {
            var uploadResults = new ConcurrentBag<ImageUploadResult>();
            Parallel.ForEach(pictureStreams, (pictureStream) =>
            {
                var guid = Guid.NewGuid().ToString();
                var uploadParams = new ImageUploadParams
                {
                    PublicId = guid,
                    File = new FileDescription(guid, pictureStream),
                    Folder = $"{itemId}",
                };
                var uploadResult = this.cloudinary.UploadLarge(uploadParams);
                uploadResults.Add(uploadResult);
            });

            var picturesToAdd = uploadResults.Select(picture => new Picture
            {
                Id = picture.PublicId.Substring(picture.PublicId.LastIndexOf('/') + 1),
                ItemId = itemId,
                Url = picture.SecureUri.AbsoluteUri
            }).ToList();

            await this.Context.Pictures.AddRangeAsync(picturesToAdd);
            await this.Context.SaveChangesAsync();
            return uploadResults;
        }
    }
}
