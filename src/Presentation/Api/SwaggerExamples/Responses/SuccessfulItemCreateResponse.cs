namespace Api.SwaggerExamples.Responses
{
    using System;
    using Application.Common.Models;
    using Application.Items.Commands;
    using Swashbuckle.AspNetCore.Filters;

    public class SuccessfulItemCreateResponse : IExamplesProvider<Response<ItemResponseModel>>
    {
        public Response<ItemResponseModel> GetExamples()
            => new Response<ItemResponseModel>(new ItemResponseModel(Guid.NewGuid()));
    }
}
