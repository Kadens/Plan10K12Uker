using Microsoft.EntityFrameworkCore;
using Plan10K12Uker.Api.Data;

namespace Plan10K12Uker.Api.Tests;

public class PlanSeedDataTests
{
    [Fact]
    public void Blocks_cover_weeks_1_to_12_without_gaps()
    {
        var weeks = PlanSeedData.Blocks
            .SelectMany(b => Enumerable.Range(b.FirstWeek, b.LastWeek - b.FirstWeek + 1))
            .Order();

        Assert.Equal(Enumerable.Range(1, 12), weeks);
    }

    [Fact]
    public void Every_block_has_one_session_per_weekday()
    {
        foreach (var block in PlanSeedData.Blocks)
        {
            var days = PlanSeedData.Sessions.Where(s => s.BlockId == block.Id).Select(s => s.DayOfWeek).Order();
            Assert.Equal(Enum.GetValues<DayOfWeek>().Order(), days);
        }
    }

    [Fact]
    public void Nutrition_has_guidelines_for_every_meal()
    {
        Assert.Equal(5, PlanSeedData.Nutrition.Count);
        Assert.All(PlanSeedData.Nutrition, n => Assert.False(string.IsNullOrWhiteSpace(n.Description)));
    }

    [Fact]
    public void Migrations_are_up_to_date_with_seed_data_and_model()
    {
        // Fails when data/plan10k12uker.json or the model changes without a new migration.
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=unused")
            .UseSnakeCaseNamingConvention()
            .Options;
        using var db = new AppDbContext(options);

        Assert.False(db.Database.HasPendingModelChanges(),
            "Run: dotnet ef migrations add <Name> --project src/Plan10K12Uker.Api --output-dir Data/Migrations");
    }
}
