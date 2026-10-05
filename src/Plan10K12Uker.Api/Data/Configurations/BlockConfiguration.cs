using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plan10K12Uker.Api.Domain;

namespace Plan10K12Uker.Api.Data.Configurations;

public class BlockConfiguration : IEntityTypeConfiguration<Block>
{
    public void Configure(EntityTypeBuilder<Block> builder)
    {
        builder.Property(b => b.Id).ValueGeneratedNever();
        builder.Property(b => b.Name).HasMaxLength(100);
        builder.Property(b => b.Focus).HasMaxLength(500);
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_blocks_weeks", "first_week >= 1 AND last_week <= 12 AND first_week <= last_week");
        });

        builder.HasMany(b => b.Sessions)
            .WithOne(s => s.Block)
            .HasForeignKey(s => s.BlockId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(PlanSeedData.Blocks.Select(b => new
        {
            b.Id,
            b.Name,
            b.Focus,
            b.FirstWeek,
            b.LastWeek,
        }));
    }
}
