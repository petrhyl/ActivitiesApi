using Domain.Core;
using Application.Mapping;
using Contracts.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Repositories;
using Application.Services.Auth;

namespace Application.Activities;

public class ActivityList
{
    public class Query : IRequest<Result<List<ActivityResponse>>> { }

    public class Handler : IRequestHandler<Query, Result<List<ActivityResponse>>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IAuthService _authService;

        public Handler(IActivityRepository activityRepository, IAuthService authService)
        {
            _activityRepository = activityRepository;
            _authService = authService;
        }

        public async Task<Result<List<ActivityResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activities = await _activityRepository.GetActivities(cancellationToken);

            var activityList = activities.OrderByDescending(a => a.BeginDate).ToList();

            var response = activityList.MapToResponse(_authService.GetCurrentUserUsername());

            if (response is null)
            {
                return Result<List<ActivityResponse>>.Success(new List<ActivityResponse>());
            }

            return Result<List<ActivityResponse>>.Success(response.ToList());
        }
    }
}
