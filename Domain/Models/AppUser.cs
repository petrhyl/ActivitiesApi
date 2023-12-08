using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class AppUser: IdentityUser
{
    public string? DisplayName { get; set; }

    public string? Bio {  get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<ActivityAttendee>? Attendees { get; set; }
}

