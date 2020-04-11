namespace Persistance.Configurations
{
    using Common;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder
                .Property(p => p.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(ModelConstants.SubCategory.NameMaxLength);

            builder
                .Property(p => p.CategoryId)
                .IsRequired();
        }
    }
}
