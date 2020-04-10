namespace AuctionSystem.Services.Tests.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using Data;
    using FluentAssertions;
    using Implementations;
    using Interfaces;
    using Models.Category;
    using Xunit;

    public class CategoriesServiceTests : BaseTest
    {
        private const string SampleCategoryName = "Fashion";

        private readonly AuctionSystemDbContext dbContext;
        private readonly ICategoriesService categoriesService;

        public CategoriesServiceTests()
        {
            this.dbContext = base.DatabaseInstance;
            this.categoriesService = new CategoriesService(this.mapper, this.dbContext);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task GetAllCategoriesWithSubCategoriesAsync_ShouldReturnValidModelAndCount(int count)
        {
            // Arrange
            await this.SeedSubCategoriesAsync(count);

            // Act
            var result = await this.categoriesService.GetAllCategoriesWithSubCategoriesAsync<CategoryListingServiceModel>();

            // Assert
            result
                .Should()
                .BeAssignableTo<IEnumerable<CategoryListingServiceModel>>()
                .And
                .HaveCount(count);
        }

        #region privateMethods

        private async Task SeedSubCategoriesAsync(int count)
        {
            var categories = new List<Category>();
            for (int i = 1; i <= count; i++)
            {
                var category = new Category { Name = SampleCategoryName };
                categories.Add(category);
            }

            await this.dbContext.Categories.AddRangeAsync(categories);
            await this.dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
