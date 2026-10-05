using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plan10K12Uker.Api.Domain;

namespace Plan10K12Uker.Api.Data.Configurations;

public class PlannedSessionConfiguration : IEntityTypeConfiguration<PlannedSession>
{
    public void Configure(EntityTypeBuilder<PlannedSession> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.HasIndex(s => new { s.BlockId, s.DayOfWeek }).IsUnique();

        builder.HasData(PlanSeedData.Sessions.Select(s => new
        {
            s.Id,
            s.BlockId,
            s.DayOfWeek,
            s.Description,
        }));
    }
}
