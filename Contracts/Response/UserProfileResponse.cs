namespace Contracts.Response;

public class UserProfileResponse
{
    public required string DisplayName { get; init; }

    public required string Username { get; init; }

    public required string Email { get; set; }

    public required string Bio { get; init; }

    public required string ImageUrl { get; set; }
}
