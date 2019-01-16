namespace AuctionSystem.Services.Implementations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.SubCategory;

    public class CategoriesService : BaseService, ICategoriesService
    {
        public CategoriesService(AuctionSystemDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<T>> GetAllSubCategoriesAsync<T>()
            where T : BaseSubCategoryServiceModel
        {
            var subCategories = await this.Context.SubCategories
                .ProjectTo<T>()
                .ToArrayAsync();

            return subCategories;
        }
    }
}