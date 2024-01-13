using API.ApiEndpoints;
using Application.Profiles;
using Contracts.Request;
using Contracts.Response;
using Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfileController: BaseApiController
{
    [Authorize]
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserProfile(string username, CancellationToken cancellationToken)
    {
        return ResultOfGetMethod(await Mediator.Send(new Detail.Query(username), cancellationToken));
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditUserProfile(UserProfileRequest userProfile, CancellationToken cancellationToken)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new Edit.Command(userProfile), cancellationToken));
    }
}
