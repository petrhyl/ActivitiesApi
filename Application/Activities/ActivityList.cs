using Domain.Core;
using Application.Mapping;
using Application.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class ActivityList
{
    public class Query : IRequest<Result<List<ActivityResponse>>> { }

    public class Handler : IRequestHandler<Query, Result<List<ActivityResponse>>>
    {
        private readonly DataContext _dbContext;

        public Handler(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<ActivityResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activityList = await _dbContext.Activities.OrderByDescending(a => a.BeginDate).ToListAsync(cancellationToken);

            var categories = await _dbContext.ActivityCategories.Select(c => c).ToArrayAsync(cancellationToken);

            var response = activityList.MapToResponse(categories);

            return Result<List<ActivityResponse>>.Success(response.ToList());
        }
    }
}
