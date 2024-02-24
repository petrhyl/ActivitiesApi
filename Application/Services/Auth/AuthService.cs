using Application.Mapping;
using Application.Services.Auth.Token;
using Contracts.Request;
using Contracts.Response;
using Domain.Core;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthService(UserManager<AppUser> userManager, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<Result<AppUserResponse>> LogUserIn(LoginRequest loginRequest)
    {
        var user = await _userManager.Users.Include(u => u.Photos).SingleOrDefaultAsync(u => u.UserName == loginRequest.Username);

        if (user is null)
        {
            return Result<AppUserResponse>.Failure("User not found.");
        }

        var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

        if (!result)
        {
            return Result<AppUserResponse>.Failure("Wrong password.");
        }

        var token = _tokenService.CreateToken(user);

        return Result<AppUserResponse>.Success(user.MapToResponse(token));
    }

    public async Task<Result<AppUserResponse>> RegisterUser(RegisterRequest request)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == request.UserName))
        {
            return Result<AppUserResponse>.Failure("User name is already taken.");
        }

        if (await _userManager.Users.AnyAsync(x => x.Email == request.Email))
        {
            return Result<AppUserResponse>.Failure("This email is already taken.");
        }

        var user = request.MapToAppUser();

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new ApplicationException("User could not be created.");
        }

        var token = _tokenService.CreateToken(user);

        return Result<AppUserResponse>.Success(user.MapToResponse(token));
    }

    public async Task<Result<AppUserResponse>> GetCurrentUser(string token)
    {
        var user = await GetCurrentUser();

        if (user is null)
        {
            return Result<AppUserResponse>.Failure("User cannot be identified.");
        }

        return Result<AppUserResponse>.Success(user.MapToResponse(token));
    }

    public async Task<Result<AppUserResponse>> GetUserWithRefreshedToken()
    {
        var user = await GetCurrentUser();

        if (user is null)
        {
            return Result<AppUserResponse>.Failure("User cannot be identified.");
        }

        var token = _tokenService.CreateToken(user);

        return Result<AppUserResponse>.Success(user.MapToResponse(token));
    }

    public string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public string? GetCurrentUserUsername()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
    }

    private async Task<AppUser?> GetCurrentUser()
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return null;
        }

        return await _userManager.Users.Include(u => u.Photos.Where(p => p.IsMain)).SingleOrDefaultAsync(u => u.Id == userId);
    }
}

