namespace Api.SwaggerExamples.Responses.Items
{
    using System;
    using System.Collections.Generic;
    using Application.Common.Models;
    using Application.Items.Queries.Details;
    using Application.Pictures;
    using Swashbuckle.AspNetCore.Filters;

    public class ItemDetails : IExamplesProvider<Response<ItemDetailsResponseModel>>
    {
        public Response<ItemDetailsResponseModel> GetExamples()
            => new Response<ItemDetailsResponseModel>(new ItemDetailsResponseModel
            {
                Id = Guid.Parse("46B33009-243D-4765-872E-08D7DFB08A87"),
                Title = "Test Title_1",
                Description = "Test Description_1",
                StartingPrice = 10000.00m,
                MinIncrease = 5.00m,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(10),
                UserFullName = "Melik Pehlivanov",
                SubCategoryName = "Antiques",
                Pictures = new List<PictureResponseModel>
                {
                    new PictureResponseModel { Id = Guid.NewGuid(), Url = "Some example url here" }
                }
            });
    }
}