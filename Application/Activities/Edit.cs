using Domain.Models;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Edit
{
    public class Command : IRequest
    {
        public Activity Activity { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _dataContext;

        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _dataContext.Activities.FindAsync(new object[] { request.Activity.Id }, cancellationToken: cancellationToken);

            activity.Title = request.Activity.Title ?? activity.Title;

            if (request.Activity.BeginDate != activity.BeginDate && request.Activity.BeginDate != DateTime.MinValue)
            {
                activity.BeginDate = request.Activity.BeginDate;
            }

            activity.Category = request.Activity.Category ?? activity.Category;
            activity.Description = request.Activity.Description ?? activity.Description;
            activity.City = request.Activity.City ?? activity.City;
            activity.Venue = request.Activity.Venue ?? activity.Venue;

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
