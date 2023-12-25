namespace Contracts.Request;

public class ActivityRequest
{
    public Guid? Id { get; set; }

    public required string Title { get; init; }

    public required DateTime BeginDate { get; init; }

    public string? Description { get; init; }

    public required ActivityCategoryRequest Category { get; init; }

    public required string City { get; init; }

    public required string Venue { get; init; }

    public required bool IsActive { get; init; } = true;
}

