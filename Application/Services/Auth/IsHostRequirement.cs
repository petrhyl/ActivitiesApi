using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services.Auth;

public class IsHostRequirement:IAuthorizationRequirement{}

public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActivityAttendeeRepository _attendeeRepository;

    public IsHostRequirementHandler(IHttpContextAccessor httpContextAccessor, IActivityAttendeeRepository attendeeRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _attendeeRepository = attendeeRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return;
        }

        var activityIdFromRoute = _httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString();

        if (activityIdFromRoute is null)
        {
            return;
        }

        var activityId = Guid.Parse(activityIdFromRoute);

        var attendee = await _attendeeRepository.GetActivityAttendeeByUserId(activityId, userId);

        if(attendee is null)
        {
            return;
        }

        if (!attendee.IsHost)
        {
            return;
        }

        context.Succeed(requirement);
    }
}
