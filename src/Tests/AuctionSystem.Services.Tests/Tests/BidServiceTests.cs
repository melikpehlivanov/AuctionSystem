namespace AuctionSystem.Services.Tests.Tests
{
    using System;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using Data;
    using FluentAssertions;
    using Implementations;
    using Interfaces;
    using Models.Bid;
    using Models.Item;
    using Xunit;

    public class BidServiceTests : BaseTest
    {
        private const decimal SampleAmount = 10;
        private const string SampleUserId = "TestUserId";
        private const string SampleItemId = "TestItemId";
        private static readonly DateTime SampleMadeOnDate = DateTime.UtcNow;

        private readonly AuctionSystemDbContext dbContext;
        private readonly IBidService bidService;

        public BidServiceTests()
        {
            this.dbContext = base.DatabaseInstance;
            this.bidService = new BidService(this.mapper, this.dbContext);
        }

        [Fact]
        public async Task CreateBidAsync_WithInvalidAmount_ShouldReturnFalse()
        {
            // Arrange
            var model = new BidCreateServiceModel { Amount = -10, UserId = SampleUserId, ItemId = SampleItemId, MadeOn = SampleMadeOnDate };
            // Act
            var result = await this.bidService.CreateBidAsync(model);

            // Assert
            result
                .Should()
                .Be(false);
        }

        [Fact]
        public async Task CreateBidAsync_WithoutItem_ShouldReturnFalse()
        {
            // Arrange
            var model = new BidCreateServiceModel { Amount = SampleAmount, UserId = SampleUserId, MadeOn = SampleMadeOnDate };
            // Act
            var result = await this.bidService.CreateBidAsync(model);

            // Assert
            result
                .Should()
                .Be(false);
        }

        [Fact]
        public async Task CreateBidAsync_WithoutUser_ShouldReturnFalse()
        {
            // Arrange
            var model = new BidCreateServiceModel { Amount = SampleAmount, ItemId = SampleItemId, MadeOn = SampleMadeOnDate };
            // Act
            var result = await this.bidService.CreateBidAsync(model);

            // Assert
            result
                .Should()
                .Be(false);
        }

        [Fact]
        public async Task CreateBidAsync_WithValidModel_ShouldReturnTrue()
        {
            // Arrange
            var model = new BidCreateServiceModel { Amount = SampleAmount, UserId = SampleUserId, ItemId = SampleItemId, MadeOn = SampleMadeOnDate };
            // Act
            var result = await this.bidService.CreateBidAsync(model);

            // Assert
            result
                .Should()
                .Be(true);
        }

        [Fact]
        public async Task GetHighestBidAmountForGivenItemAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var expectedModel = new Bid { Amount = SampleAmount, ItemId = SampleItemId, UserId = SampleUserId, MadeOn = SampleMadeOnDate };
            await this.dbContext.Bids.AddAsync(expectedModel);
            await this.dbContext.Bids.AddAsync(new Bid { Amount = 1, ItemId = SampleItemId, UserId = SampleUserId, MadeOn = SampleMadeOnDate });
            await this.dbContext.SaveChangesAsync();
            // Act
            var result = await this.bidService.GetHighestBidAmountForGivenItemAsync(null);

            // Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task GetHighestBidAmountForGivenItemAsync_WithValidId_ShouldNotReturnNull()
        {
            // Arrange
            var expectedModel = new Bid { Amount = SampleAmount, ItemId = SampleItemId, UserId = SampleUserId, MadeOn = SampleMadeOnDate };
            await this.dbContext.Bids.AddAsync(expectedModel);
            await this.dbContext.Bids.AddAsync(new Bid { Amount = 1, ItemId = SampleItemId, UserId = SampleUserId, MadeOn = SampleMadeOnDate });
            await this.dbContext.SaveChangesAsync();
            // Act
            var result = await this.bidService.GetHighestBidAmountForGivenItemAsync(SampleItemId);

            // Assert
            result
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task GetHighestBidAmountForGivenItemAsync_WithValidId_ShouldReturnHighestAmount()
        {
            // Arrange
            var expectedModel = new Bid { Amount = SampleAmount, ItemId = SampleItemId, UserId = SampleUserId, MadeOn = SampleMadeOnDate };
            await this.dbContext.Bids.AddAsync(expectedModel);
            await this.dbContext.Bids.AddAsync(new Bid { Amount = 1, ItemId = SampleItemId, UserId = SampleUserId, MadeOn = SampleMadeOnDate });
            await this.dbContext.SaveChangesAsync();
            // Act
            var result = await this.bidService.GetHighestBidAmountForGivenItemAsync(SampleItemId);

            // Assert
            result
                .Should()
                .HaveValue()
                .And
                .Be(expectedModel.Amount);
        }

        [Fact]
        public void CanBid_WithInvalidInput_ShouldReturnFalse()
        {
            // Arrange
            var model = new ItemDetailsServiceModel();
            // Act
            var result = this.bidService.CanBid(model);

            // Assert
            result
                .Should()
                .BeFalse();
        }

        [Fact]
        public void CanBid_WithValidInput_ShouldReturnTrue()
        {
            // Arrange
            var model = new ItemDetailsServiceModel{EndTime = DateTime.UtcNow.AddDays(10),};
            // Act
            var result = this.bidService.CanBid(model);

            // Assert
            result
                .Should()
                .BeTrue();
        }
    }
}
