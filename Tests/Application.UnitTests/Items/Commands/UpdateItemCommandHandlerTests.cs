namespace Application.UnitTests.Items.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Items.Commands.UpdateItem;
    using Application.Pictures.Commands.UpdatePicture;
    using Common.Exceptions;
    using Common.Interfaces;
    using FluentAssertions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Setup;
    using Xunit;

    public class UpdateItemCommandHandlerTests : CommandTestBase
    {
        private readonly Mock<ICurrentUserService> currentUserServiceMock;
        private readonly UpdateItemCommandHandler handler;
        private readonly Mock<IMediator> mediatorMock;

        public UpdateItemCommandHandlerTests()
        {
            this.currentUserServiceMock = new Mock<ICurrentUserService>();
            this.currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(DataConstants.SampleUserId);
            this.mediatorMock = new Mock<IMediator>();
            this.mediatorMock
                .Setup(x => x.Send(new UpdatePictureCommand
                {
                    ItemId = It.IsAny<Guid>(),
                }, CancellationToken.None))
                .ReturnsAsync(Unit.Value);
            this.handler =
                new UpdateItemCommandHandler(this.Context, this.currentUserServiceMock.Object,
                    this.mediatorMock.Object);
        }

        [Theory]
        [InlineData("0d0942f7-7ad3-4195-b712-c63d9a2cea30")]
        [InlineData("8d3cc00e-7f8d-4da8-9a85-088acf728487")]
        [InlineData("833eb36a-ea38-45e8-ae1c-a52caca13c56")]
        public async Task Handle_Given_InvalidId_Should_Throw_NotFoundException(string id)
        {
            var command = new UpdateItemCommand {Id = Guid.Parse(id)};
            await Assert.ThrowsAsync<NotFoundException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData("d04249ac-d5d0-4de9-990b-b35a7abbaa76")]
        [InlineData("69bc1f4d-46bd-4a10-8aef-5b61234f2ce2")]
        [InlineData("2260eaa7-7146-4f30-a698-d1fafda0dda4")]
        public async Task Handle_Given_InvalidSubCategoryId_Should_Throw_BadRequestException(string subcategoryId)
        {
            var command = new UpdateItemCommand
                {Id = DataConstants.SampleItemId, SubCategoryId = Guid.Parse(subcategoryId)};
            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Given_ValidModel_Should_Not_ThrowException_And_Should_UpdateItem()
        {
            const string newTitle = "Updated title";
            var command = new UpdateItemCommand
            {
                Id = DataConstants.SampleItemId,
                Title = newTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = DateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId
            };

            await this.handler.Handle(command, CancellationToken.None);
            var result = await this.Context
                .Items
                .SingleOrDefaultAsync(i => i.Id == DataConstants.SampleItemId);

            result
                .Title
                .Should()
                .Be(newTitle);
        }
    }
}