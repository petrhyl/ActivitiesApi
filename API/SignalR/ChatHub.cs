using Application.ChatPosts;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class ChatHub : Hub { 
private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendChatPost(CreatePost.Command command)
    {
        var post = await _mediator.Send(command);

        await Clients.Group(command.ChatPost.ActivityId.ToString())
            .SendAsync("ReceiveChatPosts", post);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        var activityId = httpContext?.Request.Query["activityId"];

        if (activityId is null)
        {
            throw new BadHttpRequestException("ID of activity is not provided in URL query", 400);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, activityId.Value!);

        var result = await _mediator.Send(new ChatPostList.Query(Guid.Parse(activityId.Value!)));

        await Clients.Caller.SendAsync("LoadChatPosts", result.Value);
    }
}

