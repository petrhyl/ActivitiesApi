namespace Domain.Models;

public class UserFollowing
{
    public required string FollowerId { get; set; }

    public required AppUser Follower { get; set; }

    public required string FolloweeId { get; set; }

    public required AppUser Followee { get; set; }
}
