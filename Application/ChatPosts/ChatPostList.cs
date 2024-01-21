using Application.Repositories;
using Application.Mapping;
using Contracts.Response;
using Domain.Core;
using MediatR;

namespace Application.ChatPosts;

public class ChatPostList
{
    public record Query(Guid ActivityId):IRequest<Result<IEnumerable<ChatPostResponse>>>;

    public class Handler : IRequestHandler<Query, Result<IEnumerable<ChatPostResponse>>>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Result<IEnumerable<ChatPostResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var posts = await _activityRepository.GetChatPostsOfActivity(request.ActivityId);

            return Result<IEnumerable<ChatPostResponse>>.Success(posts.MapToRespnse());
        }
    }
}
