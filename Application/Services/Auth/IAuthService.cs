using Contracts.Request;
using Contracts.Response;
using Domain.Core;
using System.Security.Claims;

namespace Application.Services.Auth;

public interface IAuthService
{
    Task<Result<AppUserResponse>> LogUserIn(LoginRequest loginRequest);

    Task<Result<AppUserResponse>> RegisterUser(RegisterRequest request);

    Task<Result<AppUserResponse>> GetCurrentUser(string token);

    string? GetCurrentUserId();
}
