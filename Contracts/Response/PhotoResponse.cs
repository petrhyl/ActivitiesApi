namespace Contracts.Response;

public class PhotoResponse
{
    public required string Id { get; set; }

    public required string Url { get; set; }

    public required bool IsMain { get; set; } 
}
