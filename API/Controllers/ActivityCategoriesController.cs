using Application.ActivityCategories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/activity/categories")]
public class ActivityCategoriesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken token)
    {
        return ResultOfGetMethod(await Mediator.Send(new ActivityCategoryList.Query(), token));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(Guid id, CancellationToken token)
    {
        return ResultOfGetMethod(await Mediator.Send(new Details.Query { Id = id }, token));
    }
}
