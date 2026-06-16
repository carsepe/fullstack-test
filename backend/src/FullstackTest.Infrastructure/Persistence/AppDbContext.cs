using FullstackTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FullstackTest.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Provider> Providers => Set<Provider>();

    public DbSet<ProviderService> ProviderServices => Set<ProviderService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
