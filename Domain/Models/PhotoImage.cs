namespace Domain.Models;

public class PhotoImage
{
    public string? Id { get; set; }

    public string? AppUserId { get; set; }

    public required string Url { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool IsMain { get; set; } = false;
}
