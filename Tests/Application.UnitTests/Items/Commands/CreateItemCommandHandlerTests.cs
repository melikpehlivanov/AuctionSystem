namespace Application.UnitTests.Items.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Admin.Commands.CreateAdmin;
    using Application.Bids.Commands.CreateBid;
    using Application.Items.Commands.CreateItem;
    using Pictures;
    using AuctionSystem.Infrastructure.Identity;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using Pictures.Commands.CreatePicture;
    using Setup;
    using Xunit;
    using Microsoft.EntityFrameworkCore;
    using FluentAssertions;
    using Application.Common.Exceptions;
    using Application.Items.Commands;

    public class CreateItemCommandHandlerTests : CommandTestBase
    {
        private readonly IUserManager userManagerService;
        private readonly Mock<UserManager<AuctionUser>> mockedUserManager;
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
                    ItemId = It.IsAny<Guid>(),
                }, CancellationToken.None))
                .ReturnsAsync(new MultiResponse<PictureResponseModel>(new List<PictureResponseModel>()));

            this.mockedUserManager = IdentityMocker.GetMockedUserManager();
            this.userManagerService = new UserManagerService(
                this.mockedUserManager.Object,
                IdentityMocker.GetMockedRoleManager().Object,
                this.Context);

            this.handler = 
                new CreateItemCommandHandler(this.Mapper, this.Context, this.currentUserServiceMock.Object, this.mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_GivenValidModel_Should_Not_ThrowException_AndAddItemToDatabase()
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
                SubCategoryId = DataConstants.SampleSubCategoryId,
            };
            await this.handler.Handle(command, CancellationToken.None);

            var newCount = await this.Context.Items.CountAsync();

            newCount
                .Should()
                .Be(oldCount + 1);
        }

        [Fact]
        public async Task Handle_GivenValidModel_Should_Not_ThrowException_AndReturnCorrectModel()
        {
            var command = new CreateItemCommand
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = DateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
            };

            var result = await this.handler.Handle(command, CancellationToken.None);
            result
                .Data
                .Should()
                .BeAssignableTo<ItemResponseModel>();
        }

        [Fact]
        public async Task Handle_GivenInvalidModel_Should__Throw_BadRequestException()
        {
            var command = new CreateItemCommand
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
            };

            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }
    }
}
