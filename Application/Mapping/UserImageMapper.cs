using Contracts.Response;
using Domain.Models;

namespace Application.Mapping;

public static class UserImageMapper
{
    public static PhotoImage MapToPhotoImage(this ImageUploadResponse image, bool isMain, DateTime createdAt, string userId)
    {
        return new PhotoImage
        {
            Id = image.PublicId,
            Url = image.Url,
            IsMain = isMain,
            CreatedAt = createdAt,
            AppUserId = userId
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
}
