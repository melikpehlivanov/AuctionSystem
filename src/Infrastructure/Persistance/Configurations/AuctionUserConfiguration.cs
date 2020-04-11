namespace Persistance.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuctionUserConfiguration : IEntityTypeConfiguration<AuctionUser>
    {
        public void Configure(EntityTypeBuilder<AuctionUser> builder)
        {
            builder
                .ToTable("Users");

            builder
                .HasKey(p => p.Id);

            builder
                .HasMany(b => b.Bids)
                .WithOne(u => u.AuctionUser)
                .HasForeignKey(u => u.AuctionUserId);

            builder
                .HasMany(b => b.ItemsSold)
                .WithOne(u => u.AuctionUser)
                .HasForeignKey(u => u.AuctionUserId);
        }
    }
}
