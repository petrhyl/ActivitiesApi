using Application.TransferObjects.Request;
using Application.TransferObjects.Response;
using Domain.Models;

namespace Application.Mapping;

public static class UserMapper
{
    public static AppUserResponse MapToResponse(this AppUser user, string token)
    {
        return new AppUserResponse
        {
            DisplayName = user.DisplayName,
            Username = user.UserName,
            ImageUrl = null,
            Token = token
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
}

