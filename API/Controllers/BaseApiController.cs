using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();       

    protected ActionResult ResultOfGetMethod<T>(Result<T> result)
    {
        if (result.IsScucess && result.Value is not null)
        {
            return Ok(result.Value);            
        }

        if (result.IsScucess && result.Value is null)
        {
            return NotFound();
        }
        
        return BadRequest(result.Error);               
    }

    protected ActionResult ResultOfNoContentMethod<T>(Result<T> result)
    {
        if (result is null)
        {
            return NotFound();
        }

        if (result.IsScucess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    protected ActionResult ResultOfCreateMethod<T>(Result<T> result)
    {
        if (result is null)
        {
            return NotFound();
        }

        if (result.IsScucess)
        {
            return CreatedAtAction(null, null);
        }
    
        return BadRequest(result.Error);
    }
}
