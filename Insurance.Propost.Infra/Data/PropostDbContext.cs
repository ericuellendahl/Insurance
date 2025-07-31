using Insurance.Propost.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Propost.Infra.Data;

public class PropostDbContext(DbContextOptions<PropostDbContext> options) : DbContext(options)
{

    public DbSet<PropostEntity> Proposts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("insurance");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropostDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
