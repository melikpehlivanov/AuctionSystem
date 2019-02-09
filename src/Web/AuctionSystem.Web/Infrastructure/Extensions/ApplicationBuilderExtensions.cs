namespace AuctionSystem.Web.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionSystem.Models;
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedData(this IApplicationBuilder builder)
        {
            using (var serviceScope =
                builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AuctionSystemDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<AuctionUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                dbContext.Database.Migrate();
                SeedRequiredData(dbContext, userManager, roleManager).GetAwaiter().GetResult();
            }

            return builder;
        }

        private static async Task SeedRequiredData(AuctionSystemDbContext dbContext,
            UserManager<AuctionUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedCategories(dbContext);
            await SeedDefaultRoles(roleManager);
            await SeedUsers(userManager, dbContext);
            await SeedItems(dbContext);
        }

        private static async Task SeedCategories(AuctionSystemDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                var categories = File.ReadAllText(WebConstants.CategoriesPath);

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

                await dbContext.AddRangeAsync(allCategories);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedUsers(UserManager<AuctionUser> userManager, AuctionSystemDbContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                var allUsers = new List<AuctionUser>();
                for (int i = 1; i <= 2; i++)
                {
                    var user = new AuctionUser
                    {
                        Email = $"test{i}@test.com",
                        FullName = $"Test Testov{i}",
                        UserName = $"test{i}@test.com"
                    }
                        ;
                    allUsers.Add(user);
                }

                foreach (var user in allUsers)
                {
                    user.EmailConfirmed = true;

                    await userManager.CreateAsync(user, "test123");
                }

                var admin = new AuctionUser
                {
                    Email = "admin@admin.com",
                    FullName = "Admin Adminski",
                    UserName = "admin@admin.com"
                };
                await userManager.CreateAsync(admin, "admin123");
                await userManager.AddToRoleAsync(admin, WebConstants.AdministratorRole);
            }
        }

        private static async Task SeedDefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleExist = await roleManager.RoleExistsAsync(WebConstants.AdministratorRole);

            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(WebConstants.AdministratorRole));
            }
        }

        private static async Task SeedItems(AuctionSystemDbContext dbContext)
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
                            UserId = dbContext.Users.First().Id
                        };

                        i++;
                        allItems.Add(item);
                    }
                }

                await dbContext.Items.AddRangeAsync(allItems);
                await dbContext.SaveChangesAsync();
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