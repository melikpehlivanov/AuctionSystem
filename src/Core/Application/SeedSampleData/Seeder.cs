namespace Application.SeedSampleData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Seeder
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IUserManager userManager;

        public Seeder(IAuctionSystemDbContext context, IUserManager userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            await this.SeedUsers();
            await SeedCategories(this.context, cancellationToken);
            await SeedItems(this.context, this.userManager, cancellationToken);
        }

        private async Task SeedUsers()
        {
            if (!await this.context.Users.AnyAsync())
            {
                var allUsers = new List<AuctionUser>();
                for (int i = 1; i <= 2; i++)
                {
                    var user = new AuctionUser
                    {
                        Email = $"test{i}@test.com",
                        FullName = $"Test Testov{i}",
                        UserName = $"test{i}@test.com",
                        EmailConfirmed = true
                    };

                    allUsers.Add(user);
                }

                foreach (var user in allUsers)
                {
                    var user2 = await this.userManager.CreateUserAsync(user, "test123");
                }

                var admin = new AuctionUser
                {
                    Email = "admin@admin.com",
                    FullName = "Admin Adminski",
                    UserName = "admin@admin.com",
                    EmailConfirmed = true
                };

                await this.userManager.CreateUserAsync(admin, "admin123");
                await this.SeedAdminRole();
                await this.userManager.AddToRoleAsync(admin, AppConstants.AdministratorRole);
            }
        }

        private async Task SeedAdminRole()
            => await this.userManager.CreateRoleAsync(new IdentityRole(AppConstants.AdministratorRole));

        private static async Task SeedCategories(IAuctionSystemDbContext dbContext, CancellationToken cancellationToken)
        {
            if (!dbContext.Categories.Any())
            {
                var categories = await File.ReadAllTextAsync(Path.GetFullPath(AppConstants.CategoriesPath), cancellationToken);

                var deserializedCategoriesWithSubCategories =
                    JsonConvert.DeserializeObject<CategoryDto[]>(categories);

                var allCategories = deserializedCategoriesWithSubCategories.Select(deserializedCategory => new Category
                {
                    Name = deserializedCategory.Name,
                    SubCategories = deserializedCategory.SubCategoryNames.Select(deserializedSubCategory =>
                        new SubCategory
                        {
                            Name = deserializedSubCategory.Name
                        }).ToList()
                }).ToList();

                await dbContext.Categories.AddRangeAsync(allCategories, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private static async Task SeedItems(IAuctionSystemDbContext dbContext, IUserManager manager, CancellationToken cancellationToken)
        {
            if (!dbContext.Items.Any())
            {
                var random = new Random();
                var allItems = new List<Item>();
                foreach (var category in dbContext.Categories.Include(c => c.SubCategories))
                {
                    int i = 1;
                    foreach (var subCategory in category.SubCategories)
                    {
                        var startTime = DateTime.UtcNow.AddDays(random.Next(0, 5));
                        var item = new Item
                        {
                            Description = $"Test Description_{i}",
                            Title = $"Test Title_{i}",
                            StartTime = startTime,
                            EndTime = startTime.AddHours(random.Next(1, 10)),
                            StartingPrice = random.Next(10, 500),
                            MinIncrease = random.Next(1, 100),
                            SubCategoryId = subCategory.Id,
                            UserId = await manager.GetFirstUserId()
                        };

                        i++;
                        allItems.Add(item);
                    }
                }

                await dbContext.Items.AddRangeAsync(allItems, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

    internal class CategoryDto
    {
        public string Name { get; set; }

        public SubCategoryDto[] SubCategoryNames { get; set; }
    }

    internal class SubCategoryDto
    {
        public string Name { get; set; }
    }
}
