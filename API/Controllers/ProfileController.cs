using API.ApiEndpoints;
using Application.Followings;
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
    [HttpGet(ProfileEndpoints.GetProfile)]
    public async Task<IActionResult> GetUserProfile(string username, CancellationToken cancellationToken)
    {
        return ResultOfGetMethod(await Mediator.Send(new Detail.Query(username), cancellationToken));
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditUserProfile(ProfileRequest userProfile, CancellationToken cancellationToken)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new Edit.Command(userProfile), cancellationToken));
    }

    [Authorize]
    [HttpPost(ProfileEndpoints.UpdateFollowing)]
    public async Task<IActionResult> UpdateFollowing(string username, CancellationToken cancellationToken)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new FollowToggle.Command(username), cancellationToken));
    }

    [HttpGet(ProfileEndpoints.GetFollowers)]
    public async Task<IActionResult> GetFollowers(string username, CancellationToken cancellationToken)
    {
        return ResultOfGetMethod(await Mediator.Send(new FollowerList.Query(username), cancellationToken));
    }

    [HttpGet(ProfileEndpoints.GetFollowees)]
    public async Task<IActionResult> GetFollowees(string username, CancellationToken cancellationToken)
    {
        return ResultOfGetMethod(await Mediator.Send(new FolloweeList.Query(username), cancellationToken));
    }
}
