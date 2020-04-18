namespace Application.UnitTests.Pictures.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Pictures.Queries;
    using AutoMapper;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using FluentAssertions;
    using SendGrid;
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

        [Theory]
        [InlineData("asd !!")]
        [InlineData("id which does not exist")]
        [InlineData("some random other id")]
        [InlineData("f4b5269c-e284-4448-9013-ea62c4e9379f")]
        public async Task GetPictureDetails_Given_InvalidId_Should_Throw_NotFoundException(string id)
        {
            await Assert
                .ThrowsAsync<NotFoundException>(() =>
                    this.handler.Handle(new GetPictureDetailsQuery(Guid.Parse(id)), CancellationToken.None));
        }
    }
}
