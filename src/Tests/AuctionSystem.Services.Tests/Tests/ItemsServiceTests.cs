namespace AuctionSystem.Services.Tests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using Data;
    using FluentAssertions;
    using Implementations;
    using Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Models.Item;
    using Moq;
    using Xunit;

    public class ItemsServiceTests : BaseTest
    {
        private const string SampleId = "SmapleId";
        private const string SampleTitle = "SampleTitle";
        private const string SampleDescription = "Very cool item";
        private const string SampleUsername = "TestUser";
        private const decimal SampleStartingPrice = 300;
        private const decimal SampleMinIncrease = 10;
        private static readonly DateTime SampleStartTime = DateTime.UtcNow;
        static readonly DateTime SampleEndTime = DateTime.MaxValue;
        private const string SampleSubCategoryId = "SampleSubCategoryId";

        private readonly AuctionSystemDbContext dbContext;
        private readonly IItemsService itemsService;

        public ItemsServiceTests()
        {
            this.dbContext = base.DatabaseInstance;
            this.itemsService = new ItemsService(this.dbContext, Mock.Of<IPictureService>());
        }

        [Theory]
        [InlineData(" !")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnEmptyModel(string id)
        {
            // Arrange
            await this.dbContext.Items.AddAsync(new Item { Description = SampleDescription });
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.itemsService.GetByIdAsync<ItemDetailsServiceModel>(id);

            // Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCorrectModel()
        {
            // Arrange
            const string testId = "sampleTestId";
            this.dbContext.Items.Add(new Item { Id = testId });
            this.dbContext.SaveChanges();
            // Act
            var result = await this.itemsService.GetByIdAsync<ItemDetailsServiceModel>(testId);

            // Assert
            result
                .Should()
                .BeAssignableTo<ItemDetailsServiceModel>();
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCorrectEntity()
        {
            // Arrange
            const string testId = "1";
            const int count = 4;
            for (int i = 1; i <= count; i++)
            {
                this.dbContext.Items.Add(new Item { Id = i.ToString(), Title = $"Title_{i}" });

            }
            this.dbContext.SaveChanges();

            // Act
            var result = await this.itemsService.GetByIdAsync<ItemDetailsServiceModel>(testId);

            // Assert
            result
                .Should()
                .Match(x => x.As<ItemDetailsServiceModel>().Id == testId)
                .And
                .Match(x => x.As<ItemDetailsServiceModel>().Title == $"Title_{testId}");
        }

        [Fact]
        public async Task CreateAsync_WithEmptyModel_ShouldReturnNullId_AndNotInsertItemInDatabase()
        {
            // Act
            var result = await this.itemsService.CreateAsync(new ItemCreateServiceModel());

            // Assert
            result
                .Should()
                .BeNull();

            this.dbContext
                .Items
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_WithInvalidTitle_ShouldReturnNullId_AndNotInsertItemInDatabase()
        {
            // Arrange
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var invalidTitle = new string(Enumerable.Repeat(chars, 121)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var item = new ItemCreateServiceModel
            {
                Id = SampleId,
                Title = invalidTitle,
                Description = SampleDescription,
                StartingPrice = SampleStartingPrice,
                MinIncrease = SampleMinIncrease,
                StartTime = SampleStartTime,
                EndTime = SampleEndTime,
                SubCategoryId = SampleSubCategoryId,
                UserName = SampleUsername,
            };

            // Act
            var result = await this.itemsService.CreateAsync(item);

            // Assert
            result
                .Should()
                .BeNull();

            this.dbContext
                .Items
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_WithInvalidDescription_ShouldReturnNullId_AndNotInsertItemInDatabase()
        {
            // Arrange
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var invalidDescription = new string(Enumerable.Repeat(chars, 501)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var item = new ItemCreateServiceModel
            {
                Id = SampleId,
                Title = SampleTitle,
                Description = invalidDescription,
                StartingPrice = SampleStartingPrice,
                MinIncrease = SampleMinIncrease,
                StartTime = SampleStartTime,
                EndTime = SampleEndTime,
                SubCategoryId = SampleSubCategoryId,
                UserName = SampleUsername,
            };

            // Act
            var result = await this.itemsService.CreateAsync(item);

            // Assert
            result
                .Should()
                .BeNull();

            this.dbContext
                .Items
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_WithInvalidStartingPrice_ShouldReturnNullId_AndNotInsertItemInDatabase()
        {
            // Arrange
            var item = new ItemCreateServiceModel
            {
                Id = SampleId,
                Title = SampleTitle,
                Description = SampleDescription,
                StartingPrice = -1,
                MinIncrease = SampleMinIncrease,
                StartTime = SampleStartTime,
                EndTime = SampleEndTime,
                SubCategoryId = SampleSubCategoryId,
                UserName = SampleUsername,
            };

            // Act
            var result = await this.itemsService.CreateAsync(item);

            // Assert
            result
                .Should()
                .BeNull();

            this.dbContext
                .Items
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_WithInvalidMinIncrease_ShouldReturnNullId_AndNotInsertItemInDatabase()
        {
            // Arrange
            var item = new ItemCreateServiceModel
            {
                Id = SampleId,
                Title = SampleTitle,
                Description = SampleDescription,
                StartingPrice = SampleStartingPrice,
                MinIncrease = -1,
                StartTime = SampleStartTime,
                EndTime = SampleEndTime,
                SubCategoryId = SampleSubCategoryId,
                UserName = SampleUsername,
            };

            // Act
            var result = await this.itemsService.CreateAsync(item);

            // Assert
            result
                .Should()
                .BeNull();

            this.dbContext
                .Items
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_WithInvalidUsername_ShouldReturnNullId_AndNotInsertItemInDatabase()
        {
            // Arrange
            var item = new ItemCreateServiceModel
            {
                Id = SampleId,
                Title = SampleTitle,
                Description = SampleDescription,
                StartingPrice = SampleStartingPrice,
                MinIncrease = SampleMinIncrease,
                StartTime = SampleStartTime,
                EndTime = SampleEndTime,
                SubCategoryId = SampleSubCategoryId,
                UserName = SampleUsername,
            };

            // Act
            var result = await this.itemsService.CreateAsync(item);

            // Assert
            result
                .Should()
                .BeNull();

            this.dbContext
                .Items
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_WithValidModel_ShouldReturnId_AndInsertItemInDatabase()
        {
            // Arrange
            this.dbContext.Users.Add(new AuctionUser { UserName = SampleUsername });
            this.dbContext.SubCategories.Add(new SubCategory { Id = SampleSubCategoryId });
            this.dbContext.SaveChanges();
            var item = new ItemCreateServiceModel
            {
                Id = SampleId,
                Title = SampleTitle,
                Description = SampleDescription,
                StartingPrice = SampleStartingPrice,
                MinIncrease = SampleMinIncrease,
                StartTime = SampleStartTime,
                EndTime = SampleEndTime,
                SubCategoryId = SampleSubCategoryId,
                UserName = SampleUsername,
                PictFormFiles = new List<IFormFile>(),
            };

            // Act
            var result = await this.itemsService.CreateAsync(item);

            // Assert
            result
                .Should()
                .NotBeNullOrEmpty()
                .And
                .Match(x => x.As<string>() == SampleId);

            this.dbContext
                .Items
                .Should()
                .HaveCount(1);
        }

        [Fact]
        public async Task CreateAsync_WithValidModelAndPictures_ShouldReturnId_AndInsertItemInDatabase()
        {
            // Arrange
            this.dbContext.Users.Add(new AuctionUser { UserName = SampleUsername });
            this.dbContext.SubCategories.Add(new SubCategory { Id = SampleSubCategoryId });
            this.dbContext.SaveChanges();
            var item = new ItemCreateServiceModel
            {
                Id = SampleId,
                Title = SampleTitle,
                Description = SampleDescription,
                StartingPrice = SampleStartingPrice,
                MinIncrease = SampleMinIncrease,
                StartTime = SampleStartTime,
                EndTime = SampleEndTime,
                SubCategoryId = SampleSubCategoryId,
                UserName = SampleUsername,
                PictFormFiles = new List<IFormFile> { new FormFile(Stream.Null, 200000, 50, "SampleName", "SampleFileName") },
            };

            // Act
            var result = await this.itemsService.CreateAsync(item);

            // Assert
            result
                .Should()
                .NotBeNullOrEmpty()
                .And
                .Match(x => x.As<string>() == SampleId);

            this.dbContext
                .Items
                .Should()
                .HaveCount(1);
        }
    }
}
