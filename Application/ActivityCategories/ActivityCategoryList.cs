using Application.Core;
using Application.Mapping;
using Application.TransferObjects.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ActivityCategories;

public class ActivityCategoryList
{
    public class Query : IRequest<Result<List<ActivityCategoryResponse>>> { }

    public class Handler : IRequestHandler<Query, Result<List<ActivityCategoryResponse>>>
    {
        private readonly DataContext _dbContext;

        public Handler(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<ActivityCategoryResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var categories = await _dbContext.ActivityCategories.Select(c => c).ToArrayAsync(cancellationToken);

            var response = categories.MapToResponse();

            return Result<List<ActivityCategoryResponse>>.Success(response.ToList());
        }
    }
}

