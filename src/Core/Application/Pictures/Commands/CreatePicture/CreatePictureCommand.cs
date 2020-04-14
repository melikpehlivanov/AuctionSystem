namespace Application.Pictures.Commands.CreatePicture
{
    using System;
    using System.Collections.Generic;
    using Common.Models;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class CreatePictureCommand : IRequest<Response<PictureResponseModel>>
    {
        public Guid ItemId { get; set; }

        public ICollection<IFormFile> Pictures { get; set; } = new HashSet<IFormFile>();
    }
}
