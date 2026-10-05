namespace Plan10K12Uker.Api.Domain;

public class Block
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Focus { get; set; }
    public int FirstWeek { get; set; }
    public int LastWeek { get; set; }

    public List<PlannedSession> Sessions { get; set; } = [];
}
