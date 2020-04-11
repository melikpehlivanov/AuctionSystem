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
                .Property(p => p.Id)
                .HasColumnName("Id");

            builder
                .Property(p => p.Amount)
                .IsRequired();

            builder
                .Property(p => p.UserId)
                .IsRequired();

            builder
                .Property(p => p.Item)
                .IsRequired();

            builder
                .Property(p => p.MadeOn)
                .IsRequired();

        }
    }
}
