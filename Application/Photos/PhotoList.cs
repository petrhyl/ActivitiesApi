using Application.Repositories;
using Application.Mapping;
using Contracts.Response;
using Domain.Core;
using MediatR;

namespace Application.Photos;

public class PhotoList
{
    public record Query(string Username) : IRequest<Result<IEnumerable<PhotoResponse>>>;

    public class Handler : IRequestHandler<Query, Result<IEnumerable<PhotoResponse>>>
    {
        private readonly IAppUserRepository _userRepository;

        public Handler(IAppUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<IEnumerable<PhotoResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.DoesUserExistWithUsername(request.Username, cancellationToken);

            if (!userExists)
            {
                return Result<IEnumerable<PhotoResponse>>.Success(null);
            }

            var photos = await _userRepository.GetUserPhotos(request.Username, cancellationToken);

            return Result<IEnumerable<PhotoResponse>>.Success(photos.MapToResponse());

        }
    }
}
