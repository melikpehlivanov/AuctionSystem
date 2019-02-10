namespace AuctionSystem.Services.Tests.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using Data;
    using FluentAssertions;
    using Implementations;
    using Interfaces;
    using Models.AuctionUser;
    using Xunit;

    public class UserServiceTests : BaseTest
    {
        private const string SampleFullName = "Test Test";
        private const string SampleUsername = "test";

        private readonly AuctionSystemDbContext dbContext;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.dbContext = base.DatabaseInstance;
            this.userService = new UserService(this.dbContext);
        }

        [Fact]
        public async Task GetUserIdByUsernameAsync_WithCorrectUsername_ShouldReturnId()
        {
            // Arrange
            await this.dbContext.Users.AddAsync(new AuctionUser { FullName = SampleFullName, UserName = SampleUsername });
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.userService.GetUserIdByUsernameAsync(SampleUsername);

            // Assert
            result
                .Should()
                .BeAssignableTo<string>()
                .And
                .NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetUserIdByUsernameAsync_WithInvalidUsername_ShouldReturnNull()
        {
            // Arrange
            await this.dbContext.Users.AddAsync(new AuctionUser { FullName = SampleFullName, UserName = SampleUsername });
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.userService.GetUserIdByUsernameAsync(null);

            // Assert
            result
                .Should()
                .BeNullOrEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task GetAllUsers_WithValidInput_ShouldReturnCorrectModelAndCount(int count)
        {
            // Arrange
            for (int i = 0; i < count; i++)
            {
                await this.dbContext.Users.AddAsync(new AuctionUser
                {
                    Email = $"email_{i}@asd.com", EmailConfirmed = true, FullName = SampleFullName,
                    UserName = SampleUsername
                });
            }

            await this.dbContext.SaveChangesAsync();
            // Act
            var result =  await this.userService.GetAllUsersAsync<UserListingServiceModel>();

            // Assert
            result
                .Should()
                .BeAssignableTo<IEnumerable<UserListingServiceModel>>()
                .And
                .HaveCount(count);
        }
    }
}
