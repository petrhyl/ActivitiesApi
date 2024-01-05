using Application.Interfaces;
using Application.Mapping;
using Application.Services.Auth;
using Application.Services.ImageCloud;
using Contracts.Request;
using Contracts.Response;
using Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;

namespace Application.Photos;

public class Add
{
    public record Command(UserPhotoRequest UserPhoto) : IRequest<Result<Unit>>;

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IImageCloudService _imageCloudService;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAuthService _authService;

        public Handler(IImageCloudService imageCloudService, IAppUserRepository appUserRepository, IAuthService authService)
        {
            _imageCloudService = imageCloudService;
            _appUserRepository = appUserRepository;
            _authService = authService;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUserId = _authService.GetCurrentUserId();

            if (currentUserId is null)
            {
                throw new AuthenticationException("User is not authenticated.");
            }

            var photoUploadResult = await _imageCloudService.AddImage(request.UserPhoto.File);

            var photo = photoUploadResult.MapToPhotoImage(request.UserPhoto.IsMain, DateTime.Now);

            var result = await _appUserRepository.AddUserPhoto(currentUserId, photo, cancellationToken);

            if (!result)
            {
                return Result<Unit>.Failure("Cannot save the image.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
