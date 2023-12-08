using Application.Activities.Validator;
using Domain.Core;
using Application.Mapping;
using Contracts.Request;
using FluentValidation;
using MediatR;
using Application.Interfaces;

namespace Application.Activities;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public required ActivityRequest Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IActivityRepository activityRepository)
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator(activityRepository));
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>?>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Activity?.Id is null)
            {
                return Result<Unit>.Failure("Activity ID is not provided");
            }

            var activity = await _activityRepository.GetActivityById(request.Activity.Id!.Value, cancellationToken);

            if (activity is null)
            {
                return null;
            }

            activity.Title = request.Activity.Title;
            activity.Description = request.Activity.Description;
            activity.CategoryId = request.Activity.Category.Id;
            activity.BeginDate = request.Activity.BeginDate;
            activity.City = request.Activity.City;
            activity.Venue = request.Activity.Venue;

            var result = await _activityRepository.UpdateActivity(activity, cancellationToken);

            if (!result)
            {
                return Result<Unit>.Failure("Activity cannot be updated.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
