namespace Contracts.Response;

public class ChatPostResponse
{
    public required int Id { get; set; }

    public required string Content { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required string UserName { get; set; }

    public required string DisplayName { get; set; }

    public string UserImage { get; set; } = string.Empty;
}
