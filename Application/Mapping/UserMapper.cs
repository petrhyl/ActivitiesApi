using Contracts.Request;
using Contracts.Response;
using Domain.Models;
using Newtonsoft.Json.Linq;

namespace Application.Mapping;

public static class UserMapper
{
    public static AppUserResponse MapToResponse(this AppUser user, string token)
    {
        var userResponse = user.MapToResponse();
        userResponse.Token = token;

        return userResponse;
    }

    public static AppUserResponse MapToResponse(this AppUser user)
    {
        return new AppUserResponse
        {
            DisplayName = user.DisplayName ?? string.Empty,
            Username = user.UserName!,
            Bio = user.Bio,
            ImageUrl = user.MainPhoto?.Url,
        };
    }

    public static AppUser MapToAppUser(this RegisterRequest register)
    {
        return new AppUser
        {
            DisplayName = register.DisplayName,
            Email = register.Email,
            UserName = register.UserName
        };
    }

    public static ProfileResponse MapToProfile(this AppUser user, string? currentUsername)
    {
        return new ProfileResponse
        {
            Username = user.UserName!,
            DisplayName = user.DisplayName ?? string.Empty,
            Email = user.Email!,
            Bio = user.Bio ?? string.Empty,
            ImageUrl = user.MainPhoto?.Url ?? string.Empty,
            FolloweesCount = user.Followees.Count,
            FollowersCount = user.Followers.Count,
            IsFollowedByCurrentUser = user.Followers.Any(fr => fr.UserName == currentUsername),
            IsFollowingCurrentUser = user.Followees.Any(fe => fe.UserName == currentUsername),
        };
    }

    public static ProfileResponse MapToProfileWithoutFollowing(this AppUser user)
    {
        return new ProfileResponse
        {
            Username = user.UserName!,
            DisplayName = user.DisplayName ?? string.Empty,
            Email = user.Email!,
            Bio = user.Bio ?? string.Empty,
            ImageUrl = user.MainPhoto?.Url ?? string.Empty,
            FolloweesCount = user.Followees.Count,
            FollowersCount = user.Followers.Count
        };
    }

    public static IEnumerable<ProfileResponse> MapToProfiles(this IEnumerable<AppUser> users, string? currentUsername)
    {
        return users.Select(u => u.MapToProfile(currentUsername));
    }
}

