using Application.Core;
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
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
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
                return null;
            }

            activity.ModifyToActivity(request.Activity);

            var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to edit the activity");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
