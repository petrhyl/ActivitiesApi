﻿namespace Domain.Models;
public class ActivityAttendee
{
    public required string AppUserId { get; set; }

    public AppUser? AppUser { get; set; }

    public Guid ActivityId { get; set; }

    public Activity? Activity { get; set; }

    public required bool IsHost { get; set; } = false;
}
