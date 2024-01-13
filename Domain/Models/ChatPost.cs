namespace Domain.Models;

public class ChatPost
{
    public int Id { get; set; }

    public required string Content { get; set; }

    public required AppUser Author { get; set; }

    public required Activity Activity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
