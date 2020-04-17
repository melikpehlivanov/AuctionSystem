namespace Persistance.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Common;
    using Domain.Entities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class AuctionSystemDbContextTests : IDisposable
    {
        private const string SampleUserId = "00000000-0000-0000-0000-000000000000";

        private readonly Guid sampleItemId = Guid.NewGuid();
        private readonly Guid sampleSubcategoryId = Guid.NewGuid();
        private readonly Guid sampleCategoryId = Guid.NewGuid();

        private readonly DateTime dateTime;
        private readonly Mock<IDateTime> dateTimeMock;
        private readonly Mock<ICurrentUserService> currentUserServiceMock;
        private readonly AuctionSystemDbContext context;

        public AuctionSystemDbContextTests()
        {
            this.dateTime = new DateTime(3001, 1, 1);
            this.dateTimeMock = new Mock<IDateTime>();
            this.dateTimeMock.Setup(m => m.Now).Returns(this.dateTime);

            this.currentUserServiceMock = new Mock<ICurrentUserService>();
            this.currentUserServiceMock.Setup(m => m.UserId).Returns(SampleUserId);

            var options = new DbContextOptionsBuilder<AuctionSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            this.context = new AuctionSystemDbContext(options, this.dateTimeMock.Object, this.currentUserServiceMock.Object);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewItem_ShouldSetCreatedProperties()
        {
            var item = await this.SeedItem(Guid.NewGuid());

            item
                .Created
                .Should()
                .Be(this.dateTime);
            item
                .CreatedBy
                .Should()
                .Be(SampleUserId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenExistingItem_ShouldSetLastModifiedProperties()
        {
            await this.SeedItem(this.sampleItemId);
            var product = await this.context.Items.FindAsync(this.sampleItemId);

            product.StartingPrice = 100m;

            await this.context.SaveChangesAsync();

            product
                .LastModified
                .Should()
                .NotBeNull();
            product
                .LastModified
                .Should()
                .Be(this.dateTime);
            product
                .LastModifiedBy
                .Should()
                .Be(SampleUserId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewCategory_ShouldSetCreatedProperties()
        {
            var category = await this.SeedCategory(Guid.NewGuid());

            category
                .Created
                .Should()
                .Be(this.dateTime);
            category
                .CreatedBy
                .Should()
                .Be(SampleUserId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenExistingCategory_ShouldSetLastModifiedProperties()
        {
            await this.SeedCategory(this.sampleCategoryId);
            var product = await this.context.Categories.FindAsync(this.sampleCategoryId);

            product.Name = "Some other random category name";

            await this.context.SaveChangesAsync();

            product
                .LastModified
                .Should()
                .NotBeNull();
            product
                .LastModified
                .Should()
                .Be(this.dateTime);
            product
                .LastModifiedBy
                .Should()
                .Be(SampleUserId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewSubCategory_ShouldSetCreatedProperties()
        {
            var category = await this.SeedSubCategory(Guid.NewGuid());

            category
                .Created
                .Should()
                .Be(this.dateTime);
            category
                .CreatedBy
                .Should()
                .Be(SampleUserId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenExistingSubCategory_ShouldSetLastModifiedProperties()
        {
            await this.SeedSubCategory(this.sampleCategoryId);
            var product = await this.context.SubCategories.FindAsync(this.sampleCategoryId);

            product.Name = "Some other random subcategory name";

            await this.context.SaveChangesAsync();

            product
                .LastModified
                .Should()
                .NotBeNull();
            product
                .LastModified
                .Should()
                .Be(this.dateTime);
            product
                .LastModifiedBy
                .Should()
                .Be(SampleUserId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewPicture_ShouldSetCreatedProperties()
        {
            var picture = await this.SeedPicture();

            picture
                .Created
                .Should()
                .Be(this.dateTime);
            picture
                .CreatedBy
                .Should()
                .Be(SampleUserId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewBid_ShouldSetCreatedProperties()
        {
            var bid = await this.SeedBid();

            bid
                .Created
                .Should()
                .Be(this.dateTime);
            bid
                .CreatedBy
                .Should()
                .Be(SampleUserId);
        }

        public void Dispose()
        {
            this.context?.Dispose();
        }

        private async Task<Item> SeedItem(Guid id)
        {
            var item = new Item
            {
                Id = id,
                Description = "Test Description",
                Title = "Test Title",
                StartTime = this.dateTime.AddHours(1),
                EndTime = this.dateTime.AddDays(10),
                StartingPrice = 1000,
                MinIncrease = 10,
                SubCategoryId = this.sampleSubcategoryId,
                UserId = SampleUserId,
            };

            await this.context.Items.AddAsync(item);
            await this.context.SaveChangesAsync();

            return item;
        }

        private async Task<Category> SeedCategory(Guid id)
        {
            var category = new Category
            {
                Id = id,
                Name = "random name",
                SubCategories = new HashSet<SubCategory>
                {
                    new SubCategory
                    {
                        Id = Guid.NewGuid(),
                        Name = "Some random subcategory",
                    }
                }
            };

            await this.context.Categories.AddAsync(category);
            await this.context.SaveChangesAsync();
            return category;
        }

        private async Task<SubCategory> SeedSubCategory(Guid id)
        {
            var subCategory = new SubCategory
            {
                Id = id,
                Name = "random name",
                CategoryId = Guid.NewGuid(),
            };

            await this.context.SubCategories.AddAsync(subCategory);
            await this.context.SaveChangesAsync();
            return subCategory;
        }

        private async Task<Picture> SeedPicture()
        {
            var picture = new Picture
            {
                Id = Guid.NewGuid(),
                Url = "some random url",
                ItemId = Guid.NewGuid(),
            };

            await this.context.Pictures.AddAsync(picture);
            await this.context.SaveChangesAsync();

            return picture;
        }

        private async Task<Bid> SeedBid()
        {
            var bid = new Bid
            {
                Id = Guid.NewGuid(),
                Amount = 1000m,
                UserId = SampleUserId,
                ItemId = Guid.NewGuid(),
            };

            await this.context.Bids.AddAsync(bid);
            await this.context.SaveChangesAsync();

            return bid;
        }
    }
}
