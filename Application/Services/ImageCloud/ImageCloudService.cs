using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Contracts.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Services.ImageCloud;

public class ImageCloudService : IImageCloudService
{
    private readonly ICloudinary _cloudinary;

    public ImageCloudService(ICloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<ImageUploadResponse> AddImage(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            throw new ArgumentNullException(nameof(file));
        }

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error is not null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        return new ImageUploadResponse
        {
            PublicId = uploadResult.PublicId,
            Url = uploadResult.SecureUrl.ToString()
        };
    }

    public async Task<string?> DeleteImage(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        return result.Result == "ok" ? result.Result : null;
    }
}

