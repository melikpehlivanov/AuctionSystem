namespace Application.UnitTests.Items.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Bids.Queries.Details;
    using Application.Common.Exceptions;
    using Application.Items.Queries.Details;
    using AutoMapper;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using FluentAssertions;
    using Setup;
    using Xunit;

    [Collection("QueryCollection")]
    public class GetItemDetailsQueryHandlerTests
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public GetItemDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            this.context = fixture.Context;
            this.mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetItemDetails_Should_Return_CorrectEntityAndModel()
        {
            var handler = new GetItemDetailsQueryHandler(this.context, this.mapper);
            var result = await handler.Handle(new GetItemDetailsQuery(DataConstants.SampleItemId), CancellationToken.None);

            result
                .Should()
                .BeOfType<Response<ItemDetailsResponseModel>>();
            result
                .Data
                .Id
                .Should()
                .Be(DataConstants.SampleItemId);
        }

        [Theory]
        [InlineData("0d0942f7-7ad3-4195-b712-c63d9a2cea30")]
        [InlineData("8d3cc00e-7f8d-4da8-9a85-088acf728487")]
        [InlineData("833eb36a-ea38-45e8-ae1c-a52caca13c56")]
        public async Task GetItemDetails_Given_InvalidId_Should_Throw_NotFoundException(string id)
        {
            var handler = new GetItemDetailsQueryHandler(this.context, this.mapper);
            await Assert.ThrowsAsync<NotFoundException>(() => 
                handler.Handle(new GetItemDetailsQuery(Guid.Parse(id)), CancellationToken.None));
        }
    }
}
