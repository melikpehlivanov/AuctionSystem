namespace AuctionSystem.Services.Implementations
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Data;
    using Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Models;

    public class PictureService : BaseService, IPictureService
    {
        private readonly CloudinaryOptions options;
        private readonly Cloudinary cloudinary;

        public PictureService(AuctionSystemDbContext context, IOptions<CloudinaryOptions> options)
            : base(context)
        {
            this.options = options.Value;

            var account = new Account(
                this.options.CloudName,
                this.options.ApiKey,
                this.options.ApiSecret);

            this.cloudinary = new Cloudinary(account);
        }

        public IEnumerable<ImageUploadResult> Upload(ICollection<IFormFile> pictures, string username, string title)
        {
            var uploadResults = new ConcurrentBag<ImageUploadResult>();
            Parallel.ForEach(pictures, (picture) =>
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(picture.FileName, picture.OpenReadStream()),
                    Folder = $"{username}/{title}"
                };
                var uploadResult = this.cloudinary.UploadLarge(uploadParams);
                uploadResults.Add(uploadResult);
            });

            return uploadResults;
        }
    }
}
