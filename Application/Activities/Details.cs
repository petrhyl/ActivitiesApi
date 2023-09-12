using Application.Core;
using Application.Mapping;
using Application.Response;
using Domain.Models;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Details
{
    public class Query : IRequest<Result<ActivityResponse>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ActivityResponse>>
    {
        private readonly DataContext _dataContext;

        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<ActivityResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _dataContext.Activities.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            if (activity is null)
            {
                throw new ArgumentException("some error");
            }

            var category = await _dataContext.ActivityCategories.FindAsync(new object[] { activity.CategoryId }, cancellationToken);

            var response = activity.MapToResponse(category);

            return Result<ActivityResponse>.Success(response);
        }
    }
}
