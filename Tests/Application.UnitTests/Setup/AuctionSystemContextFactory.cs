namespace Application.UnitTests.Setup
{
    using System;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Persistance;

    public class AuctionSystemContextFactory
    {
        public static AuctionSystemDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AuctionSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AuctionSystemDbContext(options);

            context.Database.EnsureCreated();

            context.Users.AddRange(
                new AuctionUser
                {
                    Email = "test@test.com",
                    FullName = "Test Testov",
                    UserName = "test@test.com",
                    EmailConfirmed = true
                },
                new AuctionUser
                {
                    Email = "admin@admin.com",
                    FullName = "Admin admin",
                    UserName = "admin@admin.com",
                    EmailConfirmed = true
                });

            context.Roles.Add(new IdentityRole(AppConstants.AdministratorRole));

            context.SaveChanges();

            return context;
        }

        public static void Destroy(AuctionSystemDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}