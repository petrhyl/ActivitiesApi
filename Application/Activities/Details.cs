using Application.Repositories;
using Application.Mapping;
using Contracts.Response;
using Domain.Core;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Services.Auth;

namespace Application.Activities;

public class Details
{
    public record Query(Guid Id) : IRequest<Result<ActivityResponse>>;

    public class Handler : IRequestHandler<Query, Result<ActivityResponse>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IAuthService _authService;

        public Handler(IActivityRepository activityRepository, IAuthService authService)
        {
            _activityRepository = activityRepository;
            _authService = authService;
        }

        public async Task<Result<ActivityResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _activityRepository.GetActivityById(request.Id, cancellationToken);

            var response = activity?.MapToResponse(_authService.GetCurrentUserUsername());

            return Result<ActivityResponse>.Success(response);
        }
    }
}
