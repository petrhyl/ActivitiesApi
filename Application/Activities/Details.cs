using Application.Interfaces;
using Application.Mapping;
using Contracts.Response;
using Domain.Core;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities;

public class Details
{
    public record Query(Guid Id) : IRequest<Result<ActivityResponse>>;

    public class Handler : IRequestHandler<Query, Result<ActivityResponse>>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Result<ActivityResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _activityRepository.GetActivityById(request.Id);

            if (activity is null)
            {
                return Result<ActivityResponse>.Success(null);
            }

            var response = activity.MapToResponse();

            return Result<ActivityResponse>.Success(response);
        }
    }
}
