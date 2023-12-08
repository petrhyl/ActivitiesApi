using API.ApiEndpoints;
using Application.Mapping;
using Application.Services.Auth;
using Contracts.Request;
using Contracts.Response;
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

    [HttpPost(AccountEndpoint.Login)]
    public async Task<ActionResult<AppUserResponse>> Login(LoginRequest request)
    {
        var result = await _authService.LogUserIn(request);

        if (!result.IsScucess)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost(AccountEndpoint.RegisterUser)]
    public async Task<ActionResult<AppUserResponse>> RegisterUser(RegisterRequest request)
    {
        var result = await _authService.RegisterUser(request);

        if (!result.IsScucess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet(AccountEndpoint.Current)]
    [Authorize]
    public async Task<ActionResult<AppUserResponse>> GetCurrentUser([FromHeader(Name = "Authorization")] string? authenticationHeader)
    {
        if (authenticationHeader is null)
        {
            return Unauthorized();
        }

        var token = authenticationHeader;

        if (token.Contains("Bearer"))
        {
            token = token.Substring("Bearer".Length + 1);
        }

        var result = await _authService.GetCurrentUser(token);

        if (!result.IsScucess)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }
}

