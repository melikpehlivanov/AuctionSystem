namespace Application.Common.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IAuctionSystemDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<SubCategory> SubCategories { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<Bid> Bids { get; set; }
        DbSet<Picture> Pictures { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
