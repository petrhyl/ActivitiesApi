using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class AppUser : IdentityUser
{
    public required string DisplayName { get; set; }

    public string? Bio { get; set; }

    public PhotoImage? MainPhoto => Photos.FirstOrDefault(p => p.IsMain);

    public ICollection<ActivityAttendee>? Attendees { get; set; }

    public ICollection<PhotoImage> Photos { get; set; } = new List<PhotoImage>();

    public ICollection<UserFollowing> Followings { get; set; } = new List<UserFollowing>();

    public ICollection<UserFollowing> Followers { get; set; } = new List<UserFollowing> { };
}

