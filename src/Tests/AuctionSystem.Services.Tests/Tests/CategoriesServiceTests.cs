namespace AuctionSystem.Services.Tests.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using Data;
    using FluentAssertions;
    using Implementations;
    using Interfaces;
    using Models.SubCategory;
    using Xunit;

    public class CategoriesServiceTests : BaseTest
    {
        private const string SampleSubCategoryName = "Watches";

        private readonly AuctionSystemDbContext dbContext;
        private readonly ICategoriesService categoriesService;

        public CategoriesServiceTests()
        {
            this.dbContext = base.DatabaseInstance;
            this.categoriesService = new CategoriesService(this.dbContext);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task GetAllSubCategoriesAsync_ShouldReturnValidModelAndCount(int count)
        {
            // Arrange
            await this.SeedSubCategoriesAsync(count);

            // Act
            var result = await this.categoriesService.GetAllSubCategoriesAsync<SubCategoryListingServiceModel>();

            // Assert
            result
                .Should()
                .BeAssignableTo<IEnumerable<SubCategoryListingServiceModel>>()
                .And
                .HaveCount(count);
        }

        #region privateMethods

        private async Task SeedSubCategoriesAsync(int count)
        {
            var subCategories = new List<SubCategory>();
            for (int i = 1; i <= count; i++)
            {
                var subCategory = new SubCategory { Name = SampleSubCategoryName, };
                subCategories.Add(subCategory);
            }

            await this.dbContext.SubCategories.AddRangeAsync(subCategories);
            await this.dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
