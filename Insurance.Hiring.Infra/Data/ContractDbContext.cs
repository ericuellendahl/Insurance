using Insurance.Hiring.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Hiring.Infra.Data
{
    public class ContractDbContext(DbContextOptions<ContractDbContext> options) : DbContext(options)
    {
        public DbSet<ContractEntity> Contracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("insurance");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContractDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
