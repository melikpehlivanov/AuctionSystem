namespace Api.SwaggerExamples.Responses.Pictures
{
    using System;
    using Application.Common.Models;
    using Application.Pictures.Queries;
    using Swashbuckle.AspNetCore.Filters;

    public class GetPictureDetailsResponse : IExamplesProvider<Response<PictureDetailsResponseModel>>
    {
        public Response<PictureDetailsResponseModel> GetExamples()
            => new Response<PictureDetailsResponseModel>(new PictureDetailsResponseModel
            {
                Id = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                Url = "https://google.com",
                ItemUserId = Guid.NewGuid().ToString(),
            });
    }
}
