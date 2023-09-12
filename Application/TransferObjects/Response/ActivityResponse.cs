using Application.TransferObjects.Response;

namespace Application.Response;

public class ActivityResponse
{
    public Guid? Id { get; set; }

    public required string Title { get; init; }

    public required DateTime BeginDate { get; init; }

    public string Description { get; init; }

    public required ActivityCategoryResponse Category { get; init; }

    public required string City { get; init; }

    public required string Venue { get; init; }
}

