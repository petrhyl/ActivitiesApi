using System.ComponentModel.DataAnnotations;

namespace Application.TransferObjects.Request;

public class RegisterRequest
{
    public required string Email { get; init; }

    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$", ErrorMessage ="Password must contain at least one uppercase, one lowercase character and one digit and must be at least 6 characters long.")]
    public required string Password { get; init; }

    public required string DisplayName { get; init; }

    public required string UserName { get; init; }
}

