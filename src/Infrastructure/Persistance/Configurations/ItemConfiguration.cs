namespace Persistance.Configurations
{
    using Common;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder
                .Property(p => p.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(ModelConstants.Item.TitleMaxLength);

            builder
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(ModelConstants.Item.DescriptionMaxLength);

            builder
                .Property(p => p.StartingPrice)
                .IsRequired();

            builder
                .Property(p => p.MinIncrease)
                .IsRequired();

            builder
                .Property(p => p.StartTime)
                .IsRequired();

            builder
                .Property(p => p.EndTime)
                .IsRequired();

            builder
                .Property(p => p.IsEmailSent)
                .IsRequired();

            builder
                .Property(p => p.UserId)
                .IsRequired();

            builder
                .Property(p => p.SubCategoryId)
                .IsRequired();

            builder
                .HasMany(b => b.Bids)
                .WithOne(i => i.Item);

            builder
                .HasMany(b => b.Pictures)
                .WithOne(i => i.Item);
        }
    }
}
