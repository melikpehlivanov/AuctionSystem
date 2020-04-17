namespace Application.UnitTests.Bids.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using AutoMapper;
    using Setup;
    using Xunit;
    using Domain.Entities;
    using System;
    using Application.Bids.Queries.Details;
    using Common.Models;
    using FluentAssertions;
    using SendGrid;

    [Collection("QueryCollection")]
    public class GetHighestBidDetailsQueryHandlerTests
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public GetHighestBidDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            this.context = fixture.Context;
            this.mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetHighestBidDetails_Should_Return_CorrectEntityAndModel()
        {
            var expectedModel = new Bid { Id = Guid.NewGuid(), Amount = 1000, ItemId = DataConstants.SampleItemId };
            await this.context.Bids.AddAsync(new Bid { Id = Guid.NewGuid(), Amount = 5, ItemId = DataConstants.SampleItemId });
            await this.context.Bids.AddAsync(expectedModel);
            await this.context.SaveChangesAsync(CancellationToken.None);

            var handler = new GetHighestBidDetailsQueryHandler(this.context, this.mapper);
            var result = await handler.Handle(new GetHighestBidDetailsQuery(expectedModel.ItemId), CancellationToken.None);

            result
                .Should()
                .BeOfType<Response<GetHighestBidDetailsResponseModel>>();
            result
                .Data
                .Id
                .Should()
                .Be(expectedModel.Id);
        }
    }
}
