namespace Contracts.Response;

public class ProfileResponse
{
    public required string DisplayName { get; init; }

    public required string Username { get; init; }

    public required string Email { get; set; }

    public required string Bio { get; init; }

    public required string ImageUrl { get; set; }

    public bool IsCurrentUserFollowing { get; set; }

    public required long FollowersCount { get; set; }

    public required long FollowingsCount { get; set; }
}
