namespace Application.UnitTests.Setup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AuctionSystem.Infrastructure;
    using Common;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Persistance;

    public class AuctionSystemContextFactory
    {

        public static AuctionSystemDbContext Create()
        {
            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId).Returns(Constants.SampleUserId);

            var options = new DbContextOptionsBuilder<AuctionSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AuctionSystemDbContext(options, new MachineDateTime(), currentUserServiceMock.Object);

            context.Database.EnsureCreated();
            SeedUsers(context);
            SeedCategory(context);
            SeedSubCategory(context);
            SeedItems(context);

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
                Id = Constants.SampleItemId,
                Title = Constants.SampleItemTitle,
                Description = Constants.SampleItemDescription,
                StartingPrice = Constants.SampleItemStartingPrice,
                MinIncrease = Constants.SampleItemMinIncrease,
                StartTime = DateTime.UtcNow,
                EndTime = Constants.SampleItemEndTime,
                UserId = context.Users.FirstOrDefault().Id,
                SubCategoryId = context.SubCategories.FirstOrDefault().Id,
            };

            context.Items.Add(item);
            context.SaveChanges();
        }

        private static void SeedUsers(AuctionSystemDbContext context)
        {
            context.Users.AddRange(
                new AuctionUser
                {
                    Id = Constants.SampleUserId,
                    Email = "test@test.com",
                    FullName = "Test Testov",
                    UserName = "test@test.com",
                    EmailConfirmed = true
                },
                new AuctionUser
                {
                    Id = Constants.SampleAdminUserId,
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
            context.SubCategories.Add(new SubCategory { Id = Constants.SampleCategoryId });
            context.SaveChanges();
        }

        private static void SeedSubCategory(AuctionSystemDbContext context)
        {
            context.SubCategories.AddAsync(new SubCategory { Id = Constants.SampleSubCategoryId });
            context.SaveChangesAsync();
        }
    }
}