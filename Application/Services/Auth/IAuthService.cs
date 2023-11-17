using Application.TransferObjects.Request;
using Application.TransferObjects.Response;
using Domain.Core;
using System.Security.Claims;

namespace Application.Services.Auth;

public interface IAuthService
{
    Task<Result<AppUserResponse>> LogUserIn(LoginRequest loginRequest);

    Task<Result<AppUserResponse>> RegisterUser(RegisterRequest request);

    Task<Result<AppUserResponse>> GetCurrentUser(ClaimsPrincipal claims, string token);
}
