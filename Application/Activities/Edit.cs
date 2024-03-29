﻿using Application.Activities.Validator;
using Domain.Core;
using Application.Mapping;
using Contracts.Request;
using FluentValidation;
using MediatR;
using Application.Repositories;

namespace Application.Activities;

public class Edit
{
    public record Command(ActivityRequest Activity) : IRequest<Result<Unit>>;

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

            var activity = await _activityRepository.GetOnlyActivityDetailsById(request.Activity.Id, cancellationToken);

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
            activity.IsActive = request.Activity.IsActive;

            var result = await _activityRepository.UpdateActivity(activity, cancellationToken);

            if (!result)
            {
                return Result<Unit>.Failure("Activity cannot be updated.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
