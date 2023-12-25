using Application.Interfaces;
using Domain.Core;
using MediatR;

namespace Application.Activities;

public class Delete
{
    public record Command(Guid Id) : IRequest<Result<Unit>>;

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IActivityRepository _activityRepository;

        public Handler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _activityRepository.DeleteActivity(request.Id, cancellationToken);

            if (!result)
            {
                throw new ApplicationException("Failed to delete the activity.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
