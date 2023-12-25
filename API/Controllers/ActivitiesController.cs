using Application.Activities;
using Application.ActivityCategories;
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

    [HttpPut("cancel/{id}")]
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

    [HttpPut("attend/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateAttendance(Guid id)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new UpdateAttendance.Command(id)));
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken token)
    {
        return ResultOfGetMethod(await Mediator.Send(new ActivityCategoryList.Query(), token));
    }
}
