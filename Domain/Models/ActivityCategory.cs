namespace Domain.Models;

public class ActivityCategory
{
    public Guid? Id { get; set; }

    public required string Name { get; set; }

    public required string Value { get; set; }

    public string? Description { get; set; }
}

