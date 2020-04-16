namespace Application.Pictures.Commands.CreatePicture
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AppSettingsModels;
    using AutoMapper;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class CreatePictureCommandHandler : IRequestHandler<CreatePictureCommand, MultiResponse<PictureResponseModel>>
    {
        private readonly Cloudinary cloudinary;
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;
        private readonly CloudinaryOptions options;

        public CreatePictureCommandHandler(
            IAuctionSystemDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IOptions<CloudinaryOptions> options)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
            this.options = options.Value;

            var account = new Account(
                this.options.CloudName,
                this.options.ApiKey,
                this.options.ApiSecret);

            this.cloudinary = new Cloudinary(account);
        }

        public async Task<MultiResponse<PictureResponseModel>> Handle(CreatePictureCommand request, CancellationToken cancellationToken)
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

            if (!request.Pictures.Any())
            {
                // Add default picture
                var picture = new Picture { ItemId = request.ItemId, Url = AppConstants.DefaultPictureUrl };
                await this.context.Pictures.AddAsync(picture, cancellationToken);
                await this.context.SaveChangesAsync(cancellationToken);

                return new MultiResponse<PictureResponseModel>(new List<PictureResponseModel>
                    { this.mapper.Map<PictureResponseModel>(picture) });
            }

            var uploadResults = new ConcurrentBag<ImageUploadResult>();
            foreach (var picture in request.Pictures)
            {
                var guid = Guid.NewGuid().ToString();
                var uploadParams = new ImageUploadParams
                {
                    PublicId = Guid.NewGuid().ToString(),
                    File = new FileDescription(guid, picture.OpenReadStream()),
                    Folder = $"{request.ItemId}"
                };
                var uploadResult = await this.cloudinary.UploadAsync(uploadParams);
                uploadResults.Add(uploadResult);
            }

            var picturesToAdd = uploadResults.Select(picture => new Picture
            {
                Id = Guid.Parse(picture.PublicId.Substring(picture.PublicId.LastIndexOf('/') + 1)),
                ItemId = request.ItemId,
                Url = picture.SecureUri.AbsoluteUri
            }).ToList();

            await this.context.Pictures.AddRangeAsync(picturesToAdd, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);

            var result = new MultiResponse<PictureResponseModel>(picturesToAdd.Select(p => this.mapper.Map<PictureResponseModel>(p)).ToList());
            return result;
        }
    }
}