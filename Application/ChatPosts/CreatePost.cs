using Application.ChatPosts.Providers;
using Application.Interfaces;
using Application.Mapping;
using Application.Services.Auth;
using Contracts.Request;
using Contracts.Response;
using Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.ChatPosts;

public class CreatePost
{
    public record Command(ChatPostRequest ChatPost) : IRequest<Result<ChatPostResponse>>;

    public class CommandValidator : AbstractValidator<ChatPostRequest>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Content).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<ChatPostResponse>?>
    {
        private readonly IAuthService _authService;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IActivityRepository _activityRepository;

        public Handler(IAuthService authService, 
            IActivityRepository activityRepository, 
            IAppUserRepository appUserRepository)
        {
            _authService = authService;
            _activityRepository = activityRepository;
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<ChatPostResponse>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();

            if (userId is null)
            {
                return Result<ChatPostResponse>.Failure("User is not authenticated.");
            }

            var user = await _appUserRepository.GetAppUserById(userId, cancellationToken);

            if (user is null)
            {
                throw new ApplicationException("Authenticated user is not found in database.");
            }

            var activity = await _activityRepository.GetActivityById(request.ChatPost.ActivityId, cancellationToken);

            if (activity is null)
            {
                return Result<ChatPostResponse>.Failure("Activity of a post was not found."); ;
            }

            var post = request.ChatPost.MapToChatPost(user, activity);

            var result = await _activityRepository.CreateChatPost(activity, post, cancellationToken);

            if (result == false)
            {
                return Result<ChatPostResponse>.Failure("Cannot add new post to activity.");
            }

            return Result<ChatPostResponse>.Success(post.MapToResponse());
        }
    }
}
