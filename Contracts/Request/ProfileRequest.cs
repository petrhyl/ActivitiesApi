namespace Contracts.Request;

public class ProfileRequest
{
    public required string Username { get; init; }

    public required string DisplayName { get; init; }

    public required string Bio { get; init; }
}
