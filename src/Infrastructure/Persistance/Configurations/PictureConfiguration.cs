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
                .Property(p => p.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder
                .Property(p => p.Url)
                .IsRequired();

            builder
                .Property(p => p.ItemId)
                .IsRequired();
        }
    }
}
