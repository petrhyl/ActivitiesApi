namespace Contracts.Response;

public class ActivityResponse
{
    public required Guid Id { get; init; }

    public required string Title { get; init; }

    public required DateTime BeginDate { get; init; }

    public string? Description { get; init; }

    public required ActivityCategoryResponse Category { get; init; }

    public required string City { get; init; }

    public required string Venue { get; init; }

    public required bool IsActive { get; init; }

    public required IEnumerable<ActivityAttenderResponse> Attenders { get; init; } = Enumerable.Empty<ActivityAttenderResponse>();
}

