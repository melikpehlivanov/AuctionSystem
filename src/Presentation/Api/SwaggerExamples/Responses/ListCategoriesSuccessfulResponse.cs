namespace Api.SwaggerExamples.Responses
{
    using System;
    using System.Collections.Generic;
    using Application.Categories.Queries.List;
    using Application.Common.Models;
    using Swashbuckle.AspNetCore.Filters;

    public class ListCategoriesSuccessfulResponse : IExamplesProvider<MultiResponse<ListCategoriesResponseModel>>
    {
        public MultiResponse<ListCategoriesResponseModel> GetExamples()
            => new MultiResponse<ListCategoriesResponseModel>(
                new List<ListCategoriesResponseModel>()
                {
                    new ListCategoriesResponseModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Art",
                        SubCategories = new List<SubCategoriesDto>()
                        {
                            new SubCategoriesDto { Id = Guid.NewGuid(), Name = "Drawings" },
                            new SubCategoriesDto { Id = Guid.NewGuid(), Name = "Photography" },
                            new SubCategoriesDto { Id = Guid.NewGuid(), Name = "Sculptures" }
                        },
                    },
                    new ListCategoriesResponseModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Jewelry",
                        SubCategories = new List<SubCategoriesDto>()
                        {
                            new SubCategoriesDto { Id = Guid.NewGuid(), Name = "Necklaces & Pendants" },
                            new SubCategoriesDto { Id = Guid.NewGuid(), Name = "Brooches & Pins" },
                            new SubCategoriesDto { Id = Guid.NewGuid(), Name = "Earrings" },
                            new SubCategoriesDto { Id = Guid.NewGuid(), Name = "Rings" }
                    },
                    }
                });
    }
}
