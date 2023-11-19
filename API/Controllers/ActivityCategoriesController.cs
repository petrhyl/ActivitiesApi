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
}
