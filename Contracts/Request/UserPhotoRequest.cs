using Microsoft.AspNetCore.Http;

namespace Contracts.Request;

public class UserPhotoRequest
{
    public required IFormFile File { get; set; }

    public required bool IsMain { get; set; }
}
