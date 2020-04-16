namespace Application.Pictures.Commands.DeletePicture
{
    using System;
    using MediatR;

    public class DeletePictureCommand : IRequest
    {
        public Guid PictureId { get; set; }

        public Guid ItemId { get; set; }
    }
}