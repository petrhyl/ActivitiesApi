using API.ApiEndpoints;
using Application.Profiles;
using Contracts.Response;
using Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfileController: BaseApiController
{
    [HttpGet("{username}")]
    [Authorize]
    public async Task<ActionResult<UserProfileResponse>> GetUserProfile(string username, CancellationToken cancellationToken)
    {
        return ResultOfGetMethod(await Mediator.Send(new Detail.Query(username), cancellationToken));
    }
}
