using Application.Repositories;
using Application.Services.Auth;
using Domain.Core;
using MediatR;

namespace Application.Followings;

public class FollowToggle
{
    public record Command(string FolloweeUsername) : IRequest<Result<Unit>>;


    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IAuthService _authService;
        private readonly IAppUserRepository _userRepository;

        public Handler(IAuthService authService, IAppUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var followerId = _authService.GetCurrentUserId();

            if (followerId is null)
            {
                throw new UnauthorizedAccessException("Authenticated user ID could not be found.");
            }

            var follower = await _userRepository.GetAppUserById(followerId, cancellationToken);

            if (follower is null)
            {
                throw new ApplicationException("Authenticated user is not found in the system.");
            }

            var followee = await _userRepository.GetUserByUsername(request.FolloweeUsername);

            if (followee is null)
            {
                return Result<Unit>.Failure("Cannot find followee user in the system.");
            }

            if (followee.Id == followerId)
            {
                return Result<Unit>.Failure("User cannot followe yourself.");
            }

            var result = await _userRepository.UpdateFollowing(followee, follower);

            if (!result)
            {
                return Result<Unit>.Failure("Following could not be updated.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
