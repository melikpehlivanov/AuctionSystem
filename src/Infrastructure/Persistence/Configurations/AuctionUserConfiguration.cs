namespace Persistence.Configurations
{
    using Common;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuctionUserConfiguration : IEntityTypeConfiguration<AuctionUser>
    {
        public void Configure(EntityTypeBuilder<AuctionUser> builder)
        {
            builder
                .Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(ModelConstants.User.FullNameMaxLength);

            builder
                .HasMany(b => b.Bids)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(b => b.ItemsSold)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}