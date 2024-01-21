using Application.Repositories;
using Application.Services.Auth;
using Domain.Core;
using MediatR;
using System.Security.Authentication;

namespace Application.Photos;

public class SetToMain
{
    public record Command(string Id) : IRequest<Result<Unit>>;

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IAppUserRepository _userRepository;
        private readonly IAuthService _authService;

        public Handler(IAppUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();

            if (userId is null)
            {
                throw new AuthenticationException("User is not authenticated.");
            }

            var result = await _userRepository.SetUserMainPhoto(request.Id, userId, cancellationToken);

            if (!result)
            {
                return Result<Unit>.Failure($"Cannot set the photo with ID {request.Id} to main.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
