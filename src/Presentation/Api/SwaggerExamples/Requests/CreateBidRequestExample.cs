namespace Api.SwaggerExamples.Requests
{
    using System;
    using Application.Bids.Commands.CreateBid;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateBidRequestExample : IExamplesProvider<CreateBidCommand>
    {
        public CreateBidCommand GetExamples()
            => new CreateBidCommand { Amount = 100000.99m, ItemId = Guid.NewGuid(), UserId = Guid.NewGuid().ToString() };
    }
}
