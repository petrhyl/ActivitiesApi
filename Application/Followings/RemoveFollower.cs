using Application.Repositories;
using Application.Services.Auth;
using Domain.Core;
using MediatR;

namespace Application.Followings;

public class RemoveFollower
{
    public record Command(string FollowerUsername) : IRequest<Result<Unit>>;


    public class Handler : IRequestHandler<Command, Result<Unit>?>
    {
        private readonly IAuthService _authService;
        private readonly IAppUserRepository _userRepository;

        public Handler(IAuthService authService, IAppUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();

            if (userId is null)
            {
                throw new UnauthorizedAccessException("Authenticated user ID could not be found.");
            }

            var followerExists = await _userRepository.DoesUserExistWithUsername(request.FollowerUsername, cancellationToken);

            if (!followerExists)
            {
                return null;
            }

            var result = await _userRepository.RemoveFollower(userId, request.FollowerUsername, cancellationToken);

            if (!result)
            {
                return Result<Unit>.Failure("Follower could not be removed");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
