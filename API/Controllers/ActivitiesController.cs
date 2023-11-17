using Application.Activities;
using Application.Request;
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
        return ResultOfGetMethod(await Mediator.Send(new Details.Query { Id = id }, token));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateActivity(ActivityRequest activity, CancellationToken token)
    {
        return ResultOfCreateMethod(await Mediator.Send(new Create.Command { Activity = activity }, token));        
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> EditActivity([FromRoute]Guid id, [FromBody]ActivityRequest activity, CancellationToken token)
    {
        activity.Id = id;
        return ResultOfNoContentMethod(await Mediator.Send(new Edit.Command { Activity = activity }, token));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken token)
    {
        return ResultOfNoContentMethod((await Mediator.Send(new Delete.Command { Id = id }, token)));
    }
}
