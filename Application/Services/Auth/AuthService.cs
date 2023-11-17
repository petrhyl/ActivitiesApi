using Application.Mapping;
using Application.Services.Auth.Token;
using Application.TransferObjects.Request;
using Application.TransferObjects.Response;
using Domain.Core;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
       

    public async Task<Result<AppUserResponse>> LogUserIn(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);

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

    public async Task<Result<AppUserResponse>> GetCurrentUser(ClaimsPrincipal claims, string token)
    {
        var userId = claims?.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (userId is null)
        {
            return Result<AppUserResponse>.Failure("User cannot be identified.");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return Result<AppUserResponse>.Failure("User cannot be identified.");
        }

        return Result<AppUserResponse>.Success(user.MapToResponse(token));
    }
}

