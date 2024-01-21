using Application.Mapping;
using Application.Repositories;
using Application.Services.Auth;
using Contracts.Response;
using Domain.Core;
using MediatR;
using System.Collections.Generic;

namespace Application.Followings;

public class FollowerList
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
            var followers = await _userRepository.GetUserFollowers(request.Username, cancellationToken);

            if (followers is null || !followers.Any())
            {
                return Result<IEnumerable<ProfileResponse>>.Success(new List<ProfileResponse>());
            }

            var profiles = followers.MapToProfiles(_authService.GetCurrentUserUsername());


            return Result<IEnumerable<ProfileResponse>>.Success(profiles);
        }
    }
}
