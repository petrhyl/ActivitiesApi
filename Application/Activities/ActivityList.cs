using Domain.Core;
using Application.Mapping;
using Contracts.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Activities;

public class ActivityList
{
    public class Query : IRequest<Result<List<ActivityResponse>>> { }

    public class Handler : IRequestHandler<Query, Result<List<ActivityResponse>>>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Result<List<ActivityResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activities = await _activityRepository.GetActivities();

            var activityList = activities.OrderByDescending(a => a.BeginDate).ToList();

            var categories = await _activityRepository.GetActivityCategories();

            var response = activityList.MapToResponse(categories);

            return Result<List<ActivityResponse>>.Success(response.ToList());
        }
    }
}
