namespace Application.ChatPosts.Providers;

public interface IHubContextProvider
{
    string? GetCurrentUserId();
}

