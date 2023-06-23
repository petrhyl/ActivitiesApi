using Application.Activities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    public ActivitiesController()
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities(CancellationToken token)
    {
        return await Mediator.Send(new ActivityList.Query(), token);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivities(Guid id, CancellationToken token)
    {
        return await Mediator.Send(new Details.Query { Id = id }, token);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity activity, CancellationToken token)
    {
        await Mediator.Send(new Create.Command { Activity = activity }, token);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, Activity activity, CancellationToken token)
    {
        activity.Id = id;
        await Mediator.Send(new Edit.Command { Activity = activity }, token);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken token)
    {
        await Mediator.Send(new Delete.Command { Id = id }, token);

        return Ok();
    }
}
