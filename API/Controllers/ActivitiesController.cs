using API.ApiEndpoints;
using Application.Activities;
using Application.ActivityCategories;
using Application.ChatPosts;
using Application.Services.Auth;
using Contracts.Request;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetActivities(CancellationToken token)
    {
        return ResultOfGetMethod(await Mediator.Send(new ActivityList.Query(), token));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivity(Guid id, CancellationToken token)
    {
        return ResultOfGetMethod(await Mediator.Send(new Details.Query(id), token));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateActivity(ActivityRequest activity, CancellationToken token)
    {
        return ResultOfCreateMethod(await Mediator.Send(new Create.Command(activity), token));        
    }

    [HttpPut("{id}")]
    [Authorize(Policy = AuthConstants.IsActivityHostPolicy)]
    public async Task<IActionResult> EditActivity([FromRoute]Guid id, [FromBody]ActivityRequest activity, CancellationToken token)
    {
        activity.Id = id;
        return ResultOfNoContentMethod(await Mediator.Send(new Edit.Command(activity), token));
    }

    [HttpPut(ActivitiesEndpoints.Cancel)]
    [Authorize(Policy = AuthConstants.IsActivityHostPolicy)]
    public async Task<IActionResult> CancelActivity(Guid id, CancellationToken token)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new Cancel.Command(id), token));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthConstants.IsActivityHostPolicy)]
    public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken token)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new Delete.Command(id), token));
    }

    [HttpPut(ActivitiesEndpoints.Attend)]
    [Authorize]
    public async Task<IActionResult> UpdateAttendance(Guid id, CancellationToken token)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new UpdateAttendance.Command(id), token));
    }

    [HttpGet(ActivitiesEndpoints.Categories)]
    public async Task<IActionResult> GetCategories(CancellationToken token)
    {
        return ResultOfGetMethod(await Mediator.Send(new ActivityCategoryList.Query(), token));
    }

    [HttpGet(ActivitiesEndpoints.ChatPosts)]
    public async Task<IActionResult> GetChatPosts(Guid id, CancellationToken token)
    {
        return ResultOfGetMethod(await Mediator.Send(new ChatPostList.Query(id), token));
    }
}
