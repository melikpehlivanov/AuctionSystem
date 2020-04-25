namespace Application.UnitTests.Pictures.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Pictures.Queries;
    using AutoMapper;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using FluentAssertions;
    using Setup;
    using Xunit;

    [Collection("QueryCollection")]
    public class GetPictureDetailsQueryHandlerTests
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        private readonly GetPictureDetailsQueryHandler handler;

        public GetPictureDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            this.context = fixture.Context;
            this.mapper = fixture.Mapper;

            this.handler = new GetPictureDetailsQueryHandler(this.context, this.mapper);
        }

        [Theory]
        [InlineData("16488cbf-0e07-4390-9eb5-9627796ffa29")]
        [InlineData("159ae2cf-7f5a-4a90-9a78-59ffe738f1a6")]
        [InlineData("a00004c9-a289-44c1-b425-71988da7d9f5")]
        [InlineData("f4b5269c-e284-4448-9013-ea62c4e9379f")]
        public async Task GetPictureDetails_Given_InvalidId_Should_Throw_NotFoundException(string id)
            => await Assert
                .ThrowsAsync<NotFoundException>(() =>
                    this.handler.Handle(new GetPictureDetailsQuery(Guid.Parse(id)), CancellationToken.None));

        [Fact]
        public async Task GetPictureDetails_Given_ValidId_Should_Return_CorrectEntityAndModel()
        {
            var expectedId = DataConstants.SamplePictureId;
            var result = await this.handler.Handle(new GetPictureDetailsQuery(expectedId), CancellationToken.None);

            result
                .Should()
                .BeOfType<Response<PictureDetailsResponseModel>>();

            result
                .Data
                .Id
                .Should()
                .Be(expectedId);
        }
    }
}