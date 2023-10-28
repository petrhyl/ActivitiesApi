using Application.TransferObjects.Request;
using Application.TransferObjects.Response;
using Domain.Core;

namespace Application.Services.Auth;

public interface IAuthService
{
    Task<Result<AppUserResponse>> LogUserIn(LoginRequest loginRequest);

    Task<Result<AppUserResponse>> RegisterUser(RegisterRequest request);
}
