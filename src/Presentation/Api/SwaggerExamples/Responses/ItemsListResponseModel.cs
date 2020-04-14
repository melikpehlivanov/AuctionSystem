namespace Api.SwaggerExamples.Responses
{
    using System;
    using System.Collections.Generic;
    using Application.Common.Models;
    using Application.Items.Queries.List;
    using Application.Pictures;
    using Swashbuckle.AspNetCore.Filters;

    public class ItemsListResponseModel : IExamplesProvider<PagedResponse<ListItemsResponseModel>>
    {
        public PagedResponse<ListItemsResponseModel> GetExamples()
            => new PagedResponse<ListItemsResponseModel>
            {
                PageNumber = 1,
                NextPage = "https://localhost:44388/api/Items?pageNumber=2&pageSize=5",
                PreviousPage = null,
                PageSize = 3,
                Data = new List<ListItemsResponseModel>
                {
                    new ListItemsResponseModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Some random title 1",
                        StartingPrice = 500m,
                        UserFullName = "test@test.com",
                        Pictures = new List<PictureResponseModel>
                        {
                            new PictureResponseModel { Id = Guid.NewGuid(), Url = "Some example url here" }
                        },
                    },
                    new ListItemsResponseModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Some random title 2",
                        StartingPrice = 1000m,
                        UserFullName = "test1@test.com",
                        Pictures = new List<PictureResponseModel>
                        {
                            new PictureResponseModel { Id = Guid.NewGuid(), Url = "Some example url here 2" }
                        },
                    },
                    new ListItemsResponseModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Some random title 3",
                        StartingPrice = 10000m,
                        UserFullName = "test2@test.com"
                    }
                }
            };
    }
}
