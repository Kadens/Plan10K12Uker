using Microsoft.EntityFrameworkCore;
using Plan10K12Uker.Api.Domain;

namespace Plan10K12Uker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Block> Blocks => Set<Block>();
    public DbSet<PlannedSession> PlannedSessions => Set<PlannedSession>();
    public DbSet<NutritionGuideline> NutritionGuidelines => Set<NutritionGuideline>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
