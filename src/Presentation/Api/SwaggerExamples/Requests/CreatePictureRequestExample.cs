namespace Api.SwaggerExamples.Requests
{
    using System;
    using System.Collections.Generic;
    using Application.Pictures.Commands.CreatePicture;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class CreatePictureRequestExample : IExamplesProvider<CreatePictureCommand>
    {
        public CreatePictureCommand GetExamples()
            => new CreatePictureCommand
            {
                ItemId = Guid.NewGuid(),
                Pictures = new List<IFormFile>()
            };
    }
}
