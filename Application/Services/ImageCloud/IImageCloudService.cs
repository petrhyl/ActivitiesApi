using Contracts.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Services.ImageCloud;

public interface IImageCloudService
{
    Task<ImageUploadResponse> AddImage(IFormFile file);

    Task<string?> DeleteImage(string publicId);
}

