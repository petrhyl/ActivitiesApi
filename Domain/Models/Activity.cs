namespace Domain.Models;

public class Activity
{
    public Guid? Id { get; set; }

    public required string Title { get; set; }

    public required DateTime BeginDate { get; set; }

    public string? Description { get; set; }

    public required string Category { get; set; }

    public required string City { get; set; }

    public required string Venue { get; set; }
}
