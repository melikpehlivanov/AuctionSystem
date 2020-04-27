namespace Persistance.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder
                .HasKey(p => p.Token);

            builder
                .Property(p => p.JwtId)
                .IsRequired();

            builder
                .Property(p => p.UserId)
                .IsRequired();
        }
    }
}
