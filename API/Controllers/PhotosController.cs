using API.ApiEndpoints;
using Application.Photos;
using Contracts.Request;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PhotosController : BaseApiController
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string isMain, CancellationToken token)
    {
        var request = new UserPhotoRequest
        {
            File = file,
            IsMain = isMain.ToLower() == "true",
        };

        return ResultOfNoContentMethod(await Mediator.Send(new Add.Command(request), token));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id, CancellationToken token)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new Delete.Command(id), token));
    }

    [HttpPost(PhotoEndpoints.SetMain)]
    [Authorize]
    public async Task<IActionResult> SetMain(string id, CancellationToken token)
    {
        return ResultOfNoContentMethod(await Mediator.Send(new SetToMain.Command(id), token));
    }

    [HttpGet(PhotoEndpoints.GetUserPhotos)]
    public async Task<IActionResult> GetUsersPhotos(string username, CancellationToken token)
    {
        return ResultOfGetMethod(await Mediator.Send(new PhotoList.Query(username), token));
    }
}
