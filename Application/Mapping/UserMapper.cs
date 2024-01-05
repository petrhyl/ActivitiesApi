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

    public static PhotoImage MapToPhotoImage(this ImageUploadResponse image, bool isMain, DateTime createdAt)
    {
        return new PhotoImage
        {
            Id = image.PublicId,
            Url = image.Url,
            IsMain = isMain,
            CreatedAt = createdAt
        };
    }

    public static PhotoResponse MapToResponse(this PhotoImage photo)
    {
        return new PhotoResponse
        {
            Id = photo.Id!,
            Url = photo.Url,
            IsMain = photo.IsMain
        };
    }

    public static IEnumerable<PhotoResponse> MapToResponse(this IEnumerable<PhotoImage> photos)
    {
        return photos.Select(p => new PhotoResponse
        {
            Id = p.Id!,
            Url = p.Url,
            IsMain = p.IsMain            
        });
    }

    public static UserProfileResponse MapToProfile(this AppUser user)
    {
        return new UserProfileResponse
        {
            Username = user.UserName!,
            DisplayName = user.DisplayName ?? string.Empty,
            Email = user.Email!,
            Bio = user.Bio,
            ImageUrl = user.MainPhoto?.Url
        };
    }
}

