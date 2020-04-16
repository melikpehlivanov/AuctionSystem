namespace Api.SwaggerExamples.Requests.Items
{
    using System;
    using Application.Items.Commands.UpdateItem;
    using Swashbuckle.AspNetCore.Filters;

    public class UpdateItemRequestExample : IExamplesProvider<UpdateItemCommand>
    {
        public UpdateItemCommand GetExamples()
            => new UpdateItemCommand
            {
                Id = Guid.NewGuid(),
                Title = "New title",
                Description = "New description",
                StartingPrice = 10000m,
                MinIncrease = 500m,
                StartTime = DateTime.UtcNow.AddDays(10),
                EndTime = DateTime.UtcNow.AddYears(1),
                SubCategoryId = Guid.NewGuid()
            };
    }
}