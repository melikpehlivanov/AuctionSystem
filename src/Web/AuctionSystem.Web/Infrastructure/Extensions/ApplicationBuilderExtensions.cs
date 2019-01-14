namespace AuctionSystem.Web.Infrastructure.Extensions
{
    using System.IO;
    using System.Linq;
    using AuctionSystem.Models;
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedData(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AuctionSystemDbContext>();

                dbContext.Database.Migrate();
                SeedRequiredData(dbContext);
            }

            return builder;
        }

        private static void SeedRequiredData(AuctionSystemDbContext dbContext)
        {
            SeedCategories(dbContext);
            SeedItems(dbContext);
        }

        private static void SeedCategories(AuctionSystemDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                var categories = File.ReadAllText(WebConstants.ManufacturersPath);

                var deserializedCategoriesWithSubCategories =
                    JsonConvert.DeserializeObject<ManufacturerDto[]>(categories);

                var allCategories = deserializedCategoriesWithSubCategories.Select(deserializedCategory => new Category
                {
                    Name = deserializedCategory.Name,
                    SubCategories = deserializedCategory.SubCategoryNames.Select(deserializedSubCategory => new SubCategory
                    {
                        Name = deserializedSubCategory.Name
                    }).ToList()
                }).ToList();

                dbContext.AddRange(allCategories);
                dbContext.SaveChanges();
            }
        }

        private static void SeedItems(AuctionSystemDbContext dbContext)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class ManufacturerDto
    {
        public string Name { get; set; }

        public SubCategoryDto[] SubCategoryNames { get; set; }
    }

    internal class SubCategoryDto
    {
        public string Name { get; set; }
    }
}
