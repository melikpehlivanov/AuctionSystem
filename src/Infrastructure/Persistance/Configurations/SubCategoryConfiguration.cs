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
                .ToTable("SubCategories");

            builder
                .HasKey(p => p.Id);

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