using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class AppUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public string? Bio { get; set; }

    public PhotoImage? MainPhoto => Photos.FirstOrDefault(p => p.IsMain);

    public ICollection<ActivityAttendee>? Attendees { get; set; }

    public ICollection<PhotoImage> Photos { get; set; } = new List<PhotoImage>();
}

