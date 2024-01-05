using Application.Interfaces;
using Application.Services.Auth;
using Application.Services.ImageCloud;
using Domain.Core;
using MediatR;
using System.Security.Authentication;

namespace Application.Photos;

public class Delete
{
    public record Command(string Id) : IRequest<Result<Unit>>;

    public class Handler : IRequestHandler<Command, Result<Unit>?>
    {
        private readonly IImageCloudService _imageCloudService;
        private readonly IAppUserRepository _userRepository;
        private readonly IAuthService _authService;

        public Handler(IAppUserRepository userRepository, IImageCloudService imageCloudService, IAuthService authService)
        {
            _userRepository = userRepository;
            _imageCloudService = imageCloudService;
            _authService = authService;
        }

        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();

            if (userId is null)
            {
                throw new AuthenticationException("User is not authenticated.");
            }

            var photo = await _userRepository.GetUserPhotoById(request.Id, userId, cancellationToken);

            if (photo is null)
            {
                return null;
            }

            if (photo.IsMain)
            {
                return Result<Unit>.Failure("You cannot delete your main photo.");
            }

            var result = await _imageCloudService.DeleteImage(request.Id);

            if (result is null)
            {
                return Result<Unit>.Failure("Problem with deleting the image from a storage system.");
            }

            var dataResult = await _userRepository.DeleteUserPhoto(request.Id, userId, cancellationToken);

            if (!dataResult)
            {
                return Result<Unit>.Failure("Problem with deleting the image information from users profile");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
