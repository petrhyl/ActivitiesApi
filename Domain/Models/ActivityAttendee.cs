namespace Domain.Models;
public class ActivityAttendee
{
    public required string AppUserId { get; set; }

    public required AppUser AppUser { get; set; }

    public required Guid ActivityId { get; set; }

    public Activity? Activity { get; set; }

    public required bool IsHost { get; set; } = false;
}
