using Contracts.Request;
using Contracts.Response;
using Domain.Models;

namespace Application.Mapping;

public static class ChatPostMapper
{
    public static ChatPostResponse MapToResponse(this ChatPost chatPost)
    {
        return new ChatPostResponse
        {
            Id = chatPost.Id,
            Content = chatPost.Content,
            CreatedAt = chatPost.CreatedAt,
            UserName = chatPost.Author.UserName!,
            DisplayName = chatPost.Author.DisplayName,
            UserImage = chatPost.Author.MainPhoto?.Url
        };
    }

    public static ChatPost MapToChatPost(this ChatPostRequest chatPostRequest, AppUser author, Activity activity)
    {
        return new ChatPost
        {
            Activity = activity,
            Author = author,
            Content = chatPostRequest.Content
        };
    }

    public static IEnumerable<ChatPostResponse> MapToRespnse(this IEnumerable<ChatPost> chatPosts)
    {
        return chatPosts.Select(ch =>
        {
            return ch.MapToResponse();
        });
    }
}
