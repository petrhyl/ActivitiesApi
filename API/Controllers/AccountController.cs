using API.ApiEndpoints;
using API.Services;
using Application.Mapping;
using Application.TransferObjects.Request;
using Application.TransferObjects.Response;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost(UserEndpoint.Login)]
    public async Task<ActionResult<AppUserResponse>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized();
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            return Unauthorized();
        }

        return user.MapToResponse(_tokenService.CreateToken(user));
    }

    [HttpPost(UserEndpoint.RegisterUser)]
    public async Task<ActionResult<AppUserResponse>> RegisterUser(RegisterRequest request)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == request.UserName))
        {
            return BadRequest("User name is already taken.");
        }

        if (await _userManager.Users.AnyAsync(x => x.Email == request.Email))
        {
            return BadRequest("This email is already taken.");
        }

        var user = request.MapToAppUser();

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest("Problem with registering user.");
        }

        return user.MapToResponse(_tokenService.CreateToken(user));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<AppUserResponse>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

        if (user is null)
        {
            return BadRequest("User was not found.");
        }

        return user.MapToResponse(_tokenService.CreateToken(user));
    }
}

