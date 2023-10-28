using Application.Activities.Validator;
using Domain.Core;
using Application.Mapping;
using Application.Request;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public ActivityRequest Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        private readonly DataContext _dataContext;

        public CommandValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator(dataContext));
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _dataContext;

        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _dataContext.Activities.FindAsync(new object[] { request.Activity.Id }, cancellationToken: cancellationToken);

            if (activity is null)
            {
                throw new ArgumentException("Activity not found.");
            }

            activity.Title = request.Activity.Title;
            activity.Description = request.Activity.Description;
            activity.CategoryId = request.Activity.Category.Id;
            activity.BeginDate = request.Activity.BeginDate;
            activity.City = request.Activity.City;
            activity.Venue = request.Activity.Venue;

            var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                return Result<Unit>.Failure("No changes.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
