using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Plan10K12Uker.Api.Data;

namespace Plan10K12Uker.Api.Tests;

[Collection(PostgresCollection.Name)]
public class DatabaseTests(PostgresFixture fixture)
{
    [Fact]
    public async Task Migrations_seed_the_plan()
    {
        await using var scope = fixture.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var blocks = await db.Blocks.Include(b => b.Sessions).OrderBy(b => b.Id).ToListAsync();

        Assert.Equal([1, 2, 3], blocks.Select(b => b.Id));
        Assert.All(blocks, b => Assert.Equal(7, b.Sessions.Count));
        Assert.Equal(5, await db.NutritionGuidelines.CountAsync());
    }

    [Fact]
    public async Task Planned_session_can_be_found_by_week_and_weekday()
    {
        await using var scope = fixture.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var session = await db.PlannedSessions
            .Where(s => s.Block!.FirstWeek <= 6 && s.Block.LastWeek >= 6 && s.DayOfWeek == DayOfWeek.Thursday)
            .SingleAsync();

        Assert.Equal(2, session.BlockId);
        Assert.Contains("FTP", session.Description);
    }

    [Fact]
    public async Task Health_endpoint_reports_healthy_database()
    {
        using var client = fixture.Factory.CreateClient();

        var response = await client.GetAsync("/api/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }
}
