using Application.Mapping;
using Application.Repositories;
using Application.Services.Auth;
using Contracts.Response;
using Domain.Core;
using MediatR;

namespace Application.Followings;

public class FolloweeList
{
    public record Query(string Username) : IRequest<Result<IEnumerable<ProfileResponse>>>;

    public class Handler : IRequestHandler<Query, Result<IEnumerable<ProfileResponse>>>
    {
        private readonly IAppUserRepository _userRepository;
        private readonly IAuthService _authService;

        public Handler(IAppUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<Result<IEnumerable<ProfileResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var followees = await _userRepository.GetUserFollowees(request.Username, cancellationToken);

            if (followees is null || !followees.Any())
            {
                return Result<IEnumerable<ProfileResponse>>.Success(new List<ProfileResponse>());
            }

            var profiles = followees.MapToProfiles(_authService.GetCurrentUserUsername());

            return Result<IEnumerable<ProfileResponse>>.Success(profiles);
        }
    }
}
