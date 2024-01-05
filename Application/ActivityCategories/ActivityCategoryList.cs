using Domain.Core;
using Application.Mapping;
using Contracts.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.ActivityCategories;

public class ActivityCategoryList
{
    public class Query : IRequest<Result<List<ActivityCategoryResponse>>> { }

    public class Handler : IRequestHandler<Query, Result<List<ActivityCategoryResponse>>>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Result<List<ActivityCategoryResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var categories = await _activityRepository.GetActivityCategories(cancellationToken);

            var response = categories.MapToResponse();

            return Result<List<ActivityCategoryResponse>>.Success(response.ToList());
        }
    }
}

