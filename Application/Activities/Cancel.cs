using Application.Repositories;
using Contracts.Response;
using Domain.Core;
using MediatR;

namespace Application.Activities;

public class Cancel
{
    public record Command(Guid Id): IRequest<Result<Unit>>;

    public class Handler : IRequestHandler<Command, Result<Unit>?>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _activityRepository.GetOnlyActivityDetailsById(request.Id, cancellationToken);

            if (activity is null)
            {
                return null;
            }

            activity.IsActive = !activity.IsActive;

            var result = await _activityRepository.UpdateActivity(activity, cancellationToken);

            if (!result)
            {
                return Result<Unit>.Failure("Activity cannot be updated.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
    }
