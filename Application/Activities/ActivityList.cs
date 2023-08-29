using Application.Core;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class ActivityList
{
    public class Query : IRequest<Result<List<Activity>>> { }

    public class Handler : IRequestHandler<Query, Result<List<Activity>>>
    {
        private readonly DataContext _dbContext;

        public Handler(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activityList = await _dbContext.Activities.OrderByDescending(a => a.BeginDate).ToListAsync(cancellationToken);

            return Result<List<Activity>>.Success(activityList);
        }
    }
}
