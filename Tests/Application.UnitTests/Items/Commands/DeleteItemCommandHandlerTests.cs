namespace Application.UnitTests.Items.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Items.Commands.DeleteItem;
    using Common.Exceptions;
    using Common.Interfaces;
    using FluentAssertions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Setup;
    using Xunit;

    public class DeleteItemCommandHandlerTests : CommandTestBase
    {
        private readonly Mock<ICurrentUserService> currentUserServiceMock;
        private readonly Mock<IMediator> mediatorMock;

        private readonly DeleteItemCommandHandler handler;

        public DeleteItemCommandHandlerTests()
        {
            this.currentUserServiceMock = new Mock<ICurrentUserService>();
            this.currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(DataConstants.SampleUserId);
            this.mediatorMock = new Mock<IMediator>();

            this.handler =
                new DeleteItemCommandHandler(this.Context, this.currentUserServiceMock.Object, this.mediatorMock.Object);
        }

        [Theory]
        [InlineData("0d0942f7-7ad3-4195-b712-c63d9a2cea30")]
        [InlineData("8d3cc00e-7f8d-4da8-9a85-088acf728487")]
        [InlineData("833eb36a-ea38-45e8-ae1c-a52caca13c56")]
        public async Task Handle_Given_InvalidId_Should_Throw_NotFoundException(string id)
        {
            var command = new DeleteItemCommand(Guid.Parse(id));
            await Assert.ThrowsAsync<NotFoundException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Given_ValidId_Should_Not_ThrowException_And_Should_DeleteItemFromDatabase()
        {
            var oldCount = await this.Context.Items.CountAsync();
            var command = new DeleteItemCommand(DataConstants.SampleItemId);
            await this.handler.Handle(command, CancellationToken.None);

            var newCount = await this.Context.Items.CountAsync();
            newCount
                .Should()
                .Be(oldCount - 1);
        }
    }
}
