using Application.ChatPosts.Providers;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.SignalR.Providers;

public class HubContextProvider : IHubContextProvider
{
    public static HubCallerContext? Context { get; set; }

    public string? GetCurrentUserId()
    {
        return Context?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
