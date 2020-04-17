namespace Application.UnitTests.Bids.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Bids.Commands.CreateBid;
    using Application.Common.Exceptions;
    using AuctionSystem.Infrastructure;
    using Common.Interfaces;
    using global::Common;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Setup;
    using Xunit;

    public class CreateBidCommandTests : CommandTestBase
    {
        private readonly CreateBidCommandHandler handler;
        private readonly Mock<ICurrentUserService> currentUserServiceMock;

        public CreateBidCommandTests()
        {
            this.currentUserServiceMock = new Mock<ICurrentUserService>();
            this.currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(Constants.SampleUserId);

            this.handler = new CreateBidCommandHandler(
                this.Context,
                this.Mapper,
                this.currentUserServiceMock.Object,
                new MachineDateTime());
        }

        [Fact]
        public async Task Handle_GivenValidModel_Should_Not_ThrowException()
        {
            var command = new CreateBidCommand { Amount = 1000, ItemId = Constants.SampleItemId, UserId = Constants.SampleUserId };
            await this.handler.Handle(command, CancellationToken.None);
        }

        [Fact]
        public async Task Handle_GivenModel_With_WrongItemId_Should_Throw_NotFoundException()
        {
            var command = new CreateBidCommand { Amount = 1000, ItemId = Guid.Empty, UserId = Constants.SampleUserId };
            await Assert.ThrowsAsync<NotFoundException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData("asdasfhgj")]
        [InlineData("some invalid user ID")]
        [InlineData("      ")]
        [InlineData("     asdf@$%45rtygf ")]
        public async Task Handle_GivenModel_With_WrongUserId_Should_Throw_NotFoundException(string userId)
        {
            var command = new CreateBidCommand { Amount = 1000, ItemId = Constants.SampleItemId, UserId = userId };
            await Assert.ThrowsAsync<NotFoundException>(() => this.handler.Handle(command, CancellationToken.None));
        }
        
        [Fact]
        public async Task Handle_Should_ThrowBadRequestException_InCase_Bidding_DidNotStartYet()
        {
            var item = await this.Context
                .Items
                .Where(i => i.Id == Constants.SampleItemId)
                .SingleOrDefaultAsync();
            // Set starTime to be > than current time
            item.StartTime = DateTime.UtcNow.AddYears(10);
            this.Context.Update(item);
            await this.Context.SaveChangesAsync();

            var command = new CreateBidCommand { Amount = 1000, ItemId = Constants.SampleItemId, UserId = Constants.SampleUserId };
            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowBadRequestException_InCase_Bidding_HasAlreadyEnded()
        {
            var item = await this.Context
                .Items
                .Where(i => i.Id == Constants.SampleItemId)
                .SingleOrDefaultAsync();
            // Set endTime to be < than current time
            item.EndTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(10));
            this.Context.Update(item);
            await this.Context.SaveChangesAsync();

            var command = new CreateBidCommand { Amount = 1000, ItemId = Constants.SampleItemId, UserId = Constants.SampleUserId };
            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowBadRequestException_InCase_BiddingAmount_IsLower_Than_TheCurrentHighestBid()
        {
            var command = new CreateBidCommand { Amount = Constants.SampleItemStartingPrice - 1, ItemId = Constants.SampleItemId, UserId = Constants.SampleUserId };
            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }
    }
}
