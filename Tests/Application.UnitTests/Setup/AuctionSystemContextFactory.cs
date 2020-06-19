namespace Application.UnitTests.Setup
{
    using System;
    using System.Linq;
    using System.Threading;
    using AuctionSystem.Infrastructure;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Persistence;

    public class AuctionSystemContextFactory
    {
        private static readonly IDateTime DateTime = new MachineDateTime();
        
        public static AuctionSystemDbContext Create()
        {
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId).Returns(DataConstants.SampleUserId);

            var options = new DbContextOptionsBuilder<AuctionSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AuctionSystemDbContext(options, DateTime, currentUserServiceMock.Object);

            context.Database.EnsureCreated();
            SeedUsers(context);
            SeedCategory(context);
            SeedSubCategory(context);
            SeedItems(context);
            SeedPictures(context);

            return context;
        }

        public static void Destroy(AuctionSystemDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        private static void SeedItems(AuctionSystemDbContext context)
        {
            var item = new Item
            {
                Id = DataConstants.SampleItemId,
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = DateTime.UtcNow.AddDays(10),
                EndTime = DataConstants.SampleItemEndTime,
                UserId = context.Users.FirstOrDefault()?.Id,
                SubCategoryId = context.SubCategories.FirstOrDefault().Id
            };

            context.Items.Add(item);
            context.SaveChanges();
        }

        private static void SeedUsers(AuctionSystemDbContext context)
        {
            context.Users.AddRange(
                new AuctionUser
                {
                    Id = DataConstants.SampleUserId,
                    Email = "test@test.com",
                    FullName = "Test Testov",
                    UserName = "test@test.com",
                    EmailConfirmed = true
                },
                new AuctionUser
                {
                    Id = DataConstants.SampleAdminUserId,
                    Email = "admin@admin.com",
                    FullName = "Admin admin",
                    UserName = "admin@admin.com",
                    EmailConfirmed = true
                });

            context.Roles.Add(new IdentityRole(AppConstants.AdministratorRole));
            context.SaveChanges();
        }

        private static void SeedCategory(AuctionSystemDbContext context)
        {
            context.Categories.Add(new Category {Id = DataConstants.SampleCategoryId});
            context.SaveChanges();
        }

        private static void SeedSubCategory(AuctionSystemDbContext context)
        {
            context.SubCategories.AddAsync(new SubCategory {Id = DataConstants.SampleSubCategoryId});
            context.SaveChangesAsync();
        }

        private static void SeedPictures(AuctionSystemDbContext context)
        {
            context.Pictures.AddAsync(new Picture
                {Id = DataConstants.SamplePictureId, ItemId = DataConstants.SampleItemId});
            context.SaveChangesAsync(CancellationToken.None);
        }
    }
}