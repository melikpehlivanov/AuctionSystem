namespace Application.UnitTests.Pictures.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Pictures.Commands.DeletePicture;
    using AppSettingsModels;
    using Common.Exceptions;
    using Common.Interfaces;
    using Microsoft.Extensions.Options;
    using Moq;
    using Setup;
    using Xunit;

    public class DeletePictureCommandHandlerTests : CommandTestBase
    {
        private readonly Mock<ICurrentUserService> currentUserServiceMock;
        private readonly Mock<IOptions<CloudinaryOptions>> cloudinaryOptions;

        private readonly DeletePictureCommandHandler handler;

        public DeletePictureCommandHandlerTests()
        {
            this.currentUserServiceMock = new Mock<ICurrentUserService>();
            this.currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(DataConstants.SampleUserId);

            this.cloudinaryOptions = new Mock<IOptions<CloudinaryOptions>>();
            this.cloudinaryOptions.Setup(x => x.Value)
                .Returns(new CloudinaryOptions
                {
                    ApiKey = "Random api key",
                    ApiSecret = "Random api secret",
                    CloudName = "Random cloud name"
                });

            this.handler = new DeletePictureCommandHandler(this.Context, this.currentUserServiceMock.Object, this.cloudinaryOptions.Object);
        }

        [Fact]
        public async Task Handle_Given_Invalid_ItemId_Should_Throw_NotFoundException()
        {
            var command = new DeletePictureCommand { ItemId = Guid.NewGuid(), PictureId = DataConstants.SamplePictureId };
            await Assert.ThrowsAsync<NotFoundException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Given_Invalid_PictureId_Should_Throw_NotFoundException()
        {
            var command = new DeletePictureCommand { ItemId = DataConstants.SampleItemId, PictureId = Guid.NewGuid() };
            await Assert.ThrowsAsync<NotFoundException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Given_ValidModel_Should_Not_ThrowException()
        {
            var command = new DeletePictureCommand { ItemId = DataConstants.SampleItemId, PictureId = DataConstants.SamplePictureId };
            await this.handler.Handle(command, CancellationToken.None);
        }
    }
}