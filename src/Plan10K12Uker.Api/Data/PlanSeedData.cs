using System.Text.Json;
using Plan10K12Uker.Api.Domain;

namespace Plan10K12Uker.Api.Data;

/// <summary>
/// Reads data/plan10k12uker.json (embedded in the assembly) and maps it to entities with
/// stable ids. Used by HasData, so the seed data is part of the migrations and reaches
/// every database that the migrations are applied to, including the idempotent SQL script.
/// </summary>
public static class PlanSeedData
{
    private const string ResourceName = "plan10k12uker.json";

    private static readonly Lazy<PlanSeed> Seed = new(Load);

    public static IReadOnlyList<Block> Blocks => Seed.Value.Blocks;
    public static IReadOnlyList<PlannedSession> Sessions => Seed.Value.Sessions;
    public static IReadOnlyList<NutritionGuideline> Nutrition => Seed.Value.Nutrition;

    private static PlanSeed Load()
    {
        using var stream = typeof(PlanSeedData).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' not found.");
        var plan = JsonSerializer.Deserialize<PlanJson>(stream, JsonSerializerOptions.Web)
            ?? throw new InvalidOperationException($"'{ResourceName}' is empty.");

        var blocks = new List<Block>();
        var sessions = new List<PlannedSession>();
        foreach (var block in plan.Blocks)
        {
            var weeks = block.Weeks.Order().ToArray();
            if (weeks.Length == 0 || weeks[^1] - weeks[0] + 1 != weeks.Length)
                throw new InvalidOperationException($"Block {block.Id} must have consecutive weeks.");

            blocks.Add(new Block
            {
                Id = block.Id,
                Name = block.Name,
                Focus = block.Focus,
                FirstWeek = weeks[0],
                LastWeek = weeks[^1],
            });

            foreach (var (day, description) in block.Schedule)
            {
                var dayOfWeek = Enum.Parse<DayOfWeek>(day);
                sessions.Add(new PlannedSession
                {
                    // Stable id per block and weekday, e.g. block 2 Monday = 21.
                    Id = block.Id * 10 + (int)dayOfWeek,
                    BlockId = block.Id,
                    DayOfWeek = dayOfWeek,
                    Description = description,
                });
            }
        }

        var n = plan.Nutrition;
        List<NutritionGuideline> nutrition =
        [
            new() { Id = 1, Meal = Meal.Breakfast, DayType = NutritionDayType.Rest, Description = n.Breakfast.RestDays },
            new() { Id = 2, Meal = Meal.Breakfast, DayType = NutritionDayType.Training, Description = n.Breakfast.TrainingDays },
            new() { Id = 3, Meal = Meal.Lunch, DayType = NutritionDayType.Any, Description = n.Lunch },
            new() { Id = 4, Meal = Meal.Dinner, DayType = NutritionDayType.Any, Description = n.Dinner },
            new() { Id = 5, Meal = Meal.Snacks, DayType = NutritionDayType.Any, Description = n.Snacks },
        ];

        return new PlanSeed(blocks, sessions, nutrition);
    }

    private sealed record PlanSeed(List<Block> Blocks, List<PlannedSession> Sessions, List<NutritionGuideline> Nutrition);

    private sealed record PlanJson(string Name, List<BlockJson> Blocks, NutritionJson Nutrition);

    private sealed record BlockJson(int Id, string Name, int[] Weeks, string Focus, Dictionary<string, string> Schedule);

    private sealed record NutritionJson(BreakfastJson Breakfast, string Lunch, string Dinner, string Snacks);

    private sealed record BreakfastJson(string RestDays, string TrainingDays);
}
