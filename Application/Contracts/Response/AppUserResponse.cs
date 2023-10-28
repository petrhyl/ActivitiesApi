using System.ComponentModel;

namespace Application.TransferObjects.Response;

public class AppUserResponse
{
    public required string DisplayName { get; init; }

    public required string Username { get; init; }

    public string Token { get; set; }

    public string ImageUrl { get; set; }
}

