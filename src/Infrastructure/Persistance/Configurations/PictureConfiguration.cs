namespace Persistance.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder
                .ToTable("Pictures");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Url)
                .IsRequired();

            builder
                .Property(p => p.ItemId)
                .IsRequired();
        }
    }
}