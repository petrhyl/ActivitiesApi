using API.ApiEndpoints;
using Application.Mapping;
using Application.Services.Auth;
using Application.TransferObjects.Request;
using Application.TransferObjects.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost(UserEndpoint.Login)]
    public async Task<ActionResult<AppUserResponse>> Login(LoginRequest request)
    {
        var result = await _authService.LogUserIn(request);

        if (!result.IsScucess)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost(UserEndpoint.RegisterUser)]
    public async Task<ActionResult<AppUserResponse>> RegisterUser(RegisterRequest request)
    {
        var result = await _authService.RegisterUser(request);

        if (!result.IsScucess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(null, result.Value);
    }
}

