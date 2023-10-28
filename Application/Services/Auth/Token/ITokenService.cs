using Domain.Models;

namespace Application.Services.Auth.Token;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
