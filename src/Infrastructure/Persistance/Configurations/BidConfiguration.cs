namespace Persistance.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder
                .ToTable("Bids");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Amount)
                .IsRequired();

            builder
                .Property(p => p.AuctionUserId)
                .IsRequired();

            builder
                .Property(p => p.ItemId)
                .IsRequired();

            builder
                .Property(p => p.MadeOn)
                .IsRequired();
        }
    }
}
