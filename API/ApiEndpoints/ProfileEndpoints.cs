namespace API.ApiEndpoints;

public class ProfileEndpoints
{
    public const string GetProfile = "{username}";
    public const string UpdateFollowing = "following/{username}";
    public const string GetFollowers = "followers/{username}";
    public const string GetFollowees = "followees/{username}";
}
