namespace Application.UnitTests.Items.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Items.Commands;
    using Application.Items.Commands.CreateItem;
    using Application.Pictures;
    using Application.Pictures.Commands.CreatePicture;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using FluentAssertions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Setup;
    using Xunit;

    public class CreateItemCommandHandlerTests : CommandTestBase
    {
        private readonly Mock<ICurrentUserService> currentUserServiceMock;
        private readonly Mock<IMediator> mediatorMock;

        private readonly CreateItemCommandHandler handler;

        public CreateItemCommandHandlerTests()
        {
            this.currentUserServiceMock = new Mock<ICurrentUserService>();
            this.currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(DataConstants.SampleUserId);
            this.mediatorMock = new Mock<IMediator>();
            this.mediatorMock
                .Setup(x => x.Send(new CreatePictureCommand
                {
                    ItemId = It.IsAny<Guid>()
                }, CancellationToken.None))
                .ReturnsAsync(new MultiResponse<PictureResponseModel>(new List<PictureResponseModel>()));

            this.handler =
                new CreateItemCommandHandler(this.Mapper, this.Context, this.currentUserServiceMock.Object, this.mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Given_InvalidModel_Should__Throw_BadRequestException()
        {
            var command = new CreateItemCommand
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease
            };

            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Given_ValidModel_Should_Not_ThrowException_AndAddItemToDatabase()
        {
            var oldCount = await this.Context.Items.CountAsync();
            var command = new CreateItemCommand
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = DateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId
            };
            await this.handler.Handle(command, CancellationToken.None);

            var newCount = await this.Context.Items.CountAsync();

            newCount
                .Should()
                .Be(oldCount + 1);
        }

        [Fact]
        public async Task Handle_Given_ValidModel_Should_Not_ThrowException_AndReturnCorrectModel()
        {
            var command = new CreateItemCommand
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = DateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId
            };

            var result = await this.handler.Handle(command, CancellationToken.None);
            result
                .Data
                .Should()
                .BeAssignableTo<ItemResponseModel>();
        }
    }
}