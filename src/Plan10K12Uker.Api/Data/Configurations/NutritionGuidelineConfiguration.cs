using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plan10K12Uker.Api.Domain;

namespace Plan10K12Uker.Api.Data.Configurations;

public class NutritionGuidelineConfiguration : IEntityTypeConfiguration<NutritionGuideline>
{
    public void Configure(EntityTypeBuilder<NutritionGuideline> builder)
    {
        builder.Property(n => n.Id).ValueGeneratedNever();
        builder.Property(n => n.Meal).HasConversion<string>().HasMaxLength(20);
        builder.Property(n => n.DayType).HasConversion<string>().HasMaxLength(20);
        builder.Property(n => n.Description).HasMaxLength(500);
        builder.HasIndex(n => new { n.Meal, n.DayType }).IsUnique();

        builder.HasData(PlanSeedData.Nutrition);
    }
}
