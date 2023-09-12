namespace Application.TransferObjects.Response;

public class ActivityCategoryResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Value { get; set; }
}

