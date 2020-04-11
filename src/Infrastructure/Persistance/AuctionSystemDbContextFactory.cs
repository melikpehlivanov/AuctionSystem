namespace Persistance
{
    using Microsoft.EntityFrameworkCore;

    public class AuctionSystemDbContextFactory : DesignTimeDbContextFactoryBase<AuctionSystemDbContext>
    {
        protected override AuctionSystemDbContext CreateNewInstance(DbContextOptions<AuctionSystemDbContext> options)
        {
            return new AuctionSystemDbContext(options);
        }
    }
}
