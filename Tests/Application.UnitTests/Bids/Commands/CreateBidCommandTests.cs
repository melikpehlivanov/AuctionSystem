namespace Application.UnitTests.Bids.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Bids.Commands.CreateBid;
    using AuctionSystem.Infrastructure;
    using Common.Exceptions;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Setup;
    using Xunit;

    public class CreateBidCommandTests : CommandTestBase
    {
        private readonly CreateBidCommandHandler handler;
        private readonly Mock<ICurrentUserService> currentUserServiceMock;
        private readonly IDateTime dateTime;

        public CreateBidCommandTests()
        {
            var dateTimeMock = new Mock<IDateTime>();
            dateTimeMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
            dateTimeMock.Setup(x => x.Now).Returns(DateTime.Now);
            this.dateTime = dateTimeMock.Object;

            this.currentUserServiceMock = new Mock<ICurrentUserService>();
            this.currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(DataConstants.SampleUserId);

            this.handler = new CreateBidCommandHandler(
                this.Context,
                this.Mapper,
                this.currentUserServiceMock.Object,
                new MachineDateTime());
        }

        [Theory]
        [InlineData("asdasfhgj")]
        [InlineData("some invalid user ID")]
        [InlineData("      ")]
        [InlineData("     asdf@$%45rtygf ")]
        public async Task Handle_Given_Model_With_WrongUserId_Should_Throw_NotFoundException(string userId)
        {
            var command = new CreateBidCommand {Amount = 1000, ItemId = DataConstants.SampleItemId, UserId = userId};
            await Assert.ThrowsAsync<NotFoundException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Given_Model_With_WrongItemId_Should_Throw_NotFoundException()
        {
            var command = new CreateBidCommand
                {Amount = 1000, ItemId = Guid.Empty, UserId = DataConstants.SampleUserId};
            await Assert.ThrowsAsync<NotFoundException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Given_ValidModel_Should_Not_ThrowException()
        {
            var itemId = Guid.NewGuid();
            await this.Context.Items.AddAsync(new Item
            {
                Id = itemId,
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10)),
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleUserId
            });
            await this.Context.SaveChangesAsync(CancellationToken.None);

            var command = new CreateBidCommand {Amount = 1000, ItemId = itemId, UserId = DataConstants.SampleUserId};
            await this.handler.Handle(command, CancellationToken.None);
        }

        [Fact]
        public async Task Handle_Should_ThrowBadRequestException_InCase_Bidding_DidNotStartYet()
        {
            var item = await this.Context
                .Items
                .Where(i => i.Id == DataConstants.SampleItemId)
                .SingleOrDefaultAsync();
            // Set starTime to be > than current time
            item.StartTime = this.dateTime.UtcNow.AddYears(10);
            this.Context.Update(item);
            await this.Context.SaveChangesAsync();

            var command = new CreateBidCommand
                {Amount = 1000, ItemId = DataConstants.SampleItemId, UserId = DataConstants.SampleUserId};
            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowBadRequestException_InCase_Bidding_HasAlreadyEnded()
        {
            var item = await this.Context
                .Items
                .Where(i => i.Id == DataConstants.SampleItemId)
                .SingleOrDefaultAsync();
            // Set endTime to be < than current time
            item.EndTime = this.dateTime.UtcNow.Subtract(TimeSpan.FromDays(10));
            this.Context.Update(item);
            await this.Context.SaveChangesAsync();

            var command = new CreateBidCommand
                {Amount = 1000, ItemId = DataConstants.SampleItemId, UserId = DataConstants.SampleUserId};
            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task
            Handle_Should_ThrowBadRequestException_InCase_BiddingAmount_IsLower_Than_TheCurrentHighestBid()
        {
            var command = new CreateBidCommand
            {
                Amount = DataConstants.SampleItemStartingPrice - 1, ItemId = DataConstants.SampleItemId,
                UserId = DataConstants.SampleUserId
            };
            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }
    }
}