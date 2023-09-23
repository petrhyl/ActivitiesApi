using Domain.Models;

namespace API.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
