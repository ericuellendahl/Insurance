using Insurance.Hiring.Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Hiring.Infra.Configurations
{
    internal class ContractEntityConfiguration : IEntityTypeConfiguration<ContractEntity>
    {
        public void Configure(EntityTypeBuilder<ContractEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(c => c.PropostId)
                .IsRequired();

            builder.Property(c => c.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.CoverageAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.ContractDate)
                .IsRequired();
        }
    }
}
