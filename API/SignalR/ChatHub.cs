using API.SignalR.Providers;
using Application.ChatPosts;
using Application.Services.Auth;
using Contracts.Request;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.SignalR;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;

    public ChatHub(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    public async Task SendChatPost(ChatPostRequest request)
    {
        var post = await _mediator.Send(new CreatePost.Command(request));

        await Clients
            .Group(request.ActivityId.ToString())
            .SendAsync("ReceiveChatPost", post.Value);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        var activityId = httpContext?.Request.Query["activityId"];

        if (activityId is null)
        {
            await Clients.Caller.SendAsync("LoadChatPosts", "Not provided activity ID.");

            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, activityId.Value!);

        var result = await _mediator.Send(new ChatPostList.Query(Guid.Parse(activityId.Value!)));

        await Clients.Caller.SendAsync("LoadChatPosts", result.Value);
    }
}

