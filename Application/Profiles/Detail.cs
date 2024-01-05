using Application.Interfaces;
using Application.Mapping;
using Contracts.Response;
using Domain.Core;
using MediatR;

namespace Application.Profiles;

public class Detail
{
    public record Query(string Username): IRequest<Result<UserProfileResponse>>;

    public class Handler : IRequestHandler<Query, Result<UserProfileResponse>>
    {
        private readonly IAppUserRepository _userRepository;

        public Handler(IAppUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserProfileResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsername(request.Username, cancellationToken);

            if (user is null)
            {
                return Result<UserProfileResponse>.Success(null);
            }

            return Result<UserProfileResponse>.Success(user.MapToProfile());
        }
    }
}
