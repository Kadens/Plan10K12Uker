namespace Plan10K12Uker.Api.Domain;

public enum Meal
{
    Breakfast,
    Lunch,
    Dinner,
    Snacks,
}

public enum NutritionDayType
{
    Any,
    Rest,
    Training,
}

public class NutritionGuideline
{
    public int Id { get; set; }
    public Meal Meal { get; set; }
    public NutritionDayType DayType { get; set; }
    public required string Description { get; set; }
}
