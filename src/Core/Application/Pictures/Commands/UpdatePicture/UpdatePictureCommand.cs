namespace Application.Pictures.Commands.UpdatePicture
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class UpdatePictureCommand : IRequest<Unit>
    {
        public Guid ItemId { get; set; }

        public ICollection<IFormFile> PicturesToAdd { get; set; } = new HashSet<IFormFile>();

        public ICollection<Guid> PicturesToRemove { get; set; } = new HashSet<Guid>();
    }
}