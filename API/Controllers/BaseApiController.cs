﻿using Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    public IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected ActionResult RequestResult<T>(Result<T> result)
    {
        if (result.IsScucess && result.Value is null)
        {
            return NotFound();
        }

        if (result.IsScucess && result.Value is Unit)
        {
            return NoContent();
        }

        if (result.IsScucess && result.Value is not null)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    protected ActionResult ResultOfCreateMethod<T>(Result<T> result)
    {
        if (result.IsScucess)
        {
            return CreatedAtAction(null, null);
        }
    
        return BadRequest(result.Error);
    }
}
