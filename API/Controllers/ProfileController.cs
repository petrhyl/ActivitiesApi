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
        return RequestResult(await Mediator.Send(new Detail.Query(username), cancellationToken));
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditUserProfile(ProfileRequest userProfile, CancellationToken cancellationToken)
    {
        return RequestResult(await Mediator.Send(new Edit.Command(userProfile), cancellationToken));
    }

    [Authorize]
    [HttpPost(ProfileEndpoints.UpdateFollowing)]
    public async Task<IActionResult> UpdateFollowing(string username, CancellationToken cancellationToken)
    {
        return RequestResult(await Mediator.Send(new FollowToggle.Command(username), cancellationToken));
    }

    [Authorize]
    [HttpGet(ProfileEndpoints.GetFollowers)]
    public async Task<IActionResult> GetFollowers(string username, CancellationToken cancellationToken)
    {
        return RequestResult(await Mediator.Send(new FollowerList.Query(username), cancellationToken));
    }

    [Authorize]
    [HttpGet(ProfileEndpoints.GetFollowees)]
    public async Task<IActionResult> GetFollowees(string username, CancellationToken cancellationToken)
    {
        return RequestResult(await Mediator.Send(new FolloweeList.Query(username), cancellationToken));
    }

    [Authorize]
    [HttpPost(ProfileEndpoints.RemoveFollower)]
    public async Task<IActionResult> RemoveFollower(string username, CancellationToken cancellationToken)
    {
        return RequestResult(await Mediator.Send(new RemoveFollower.Command(username), cancellationToken));
    }
}
