namespace Contracts.Request;

public class ChatPostRequest
{
    public required Guid ActivityId { get; set; }

    public required string Content { get; set; }

    public string? UserId { get; set; }
}
