using Insurance.Propost.Domain.Entities;
using Insurance.Propost.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Propost.Infra.Configurations
{
    internal class PropostConfiguration : IEntityTypeConfiguration<PropostEntity>
    {
        public void Configure(EntityTypeBuilder<PropostEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Email)
                .HasConversion(
                        email => email.Address,
                        value => new Email(value))
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.CoverageAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.InsuranceType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt);
        }
    }
}
