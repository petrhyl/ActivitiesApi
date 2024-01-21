using Application.Repositories;
using Application.Mapping;
using Contracts.Response;
using Domain.Core;
using MediatR;
using Application.Services.Auth;

namespace Application.Profiles;

public class Detail
{
    public record Query(string Username): IRequest<Result<ProfileResponse>>;

    public class Handler : IRequestHandler<Query, Result<ProfileResponse>>
    {
        private readonly IAppUserRepository _userRepository;
        private readonly IAuthService _authService;

        public Handler(IAppUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<Result<ProfileResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsername(request.Username, cancellationToken);

            if (user is null)
            {
                return Result<ProfileResponse>.Success(null);
            }

            return Result<ProfileResponse>.Success(user.MapToProfile(_authService.GetCurrentUserUsername()));
        }
    }
}
