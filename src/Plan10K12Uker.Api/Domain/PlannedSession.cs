namespace Plan10K12Uker.Api.Domain;

public class PlannedSession
{
    public int Id { get; set; }
    public int BlockId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public required string Description { get; set; }

    public Block? Block { get; set; }
}
