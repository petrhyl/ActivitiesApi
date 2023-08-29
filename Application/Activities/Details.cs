using Application.Core;
using Domain.Models;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Details
{
    public class Query : IRequest<Result<Activity>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Activity>>
    {
        private readonly DataContext _dataContext;

        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _dataContext.Activities.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            return Result<Activity>.Success(activity);
        }
    }
}
