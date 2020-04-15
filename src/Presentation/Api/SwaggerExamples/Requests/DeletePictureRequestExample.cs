namespace Api.SwaggerExamples.Requests
{
    using System;
    using Application.Pictures.Commands.DeletePicture;
    using Swashbuckle.AspNetCore.Filters;

    public class DeletePictureRequestExample : IExamplesProvider<DeletePictureCommand>
    {
        public DeletePictureCommand GetExamples()
            => new DeletePictureCommand { PictureId = Guid.NewGuid(), ItemId = Guid.NewGuid() };
    }
}
