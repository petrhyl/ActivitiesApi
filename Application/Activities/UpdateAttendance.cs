using Application.Interfaces;
using Application.Services.Auth;
using Domain.Core;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Application.Activities;

public class UpdateAttendance
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>?>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAuthService _authService;

        public Handler(IAuthService authService, IActivityRepository activityRepository, IAppUserRepository appUserRepository)
        {
            _authService = authService;
            _activityRepository = activityRepository;
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _activityRepository.GetActivityById(request.Id, cancellationToken);

            if (activity is null)
            {
                return null;
            }

            var currentUserId = _authService.GetCurrentUserId();

            if (currentUserId is null)
            {
                throw new AuthenticationException("Current user is not authenticated.");
            }

            var user = await _appUserRepository.GetAppUserById(currentUserId, cancellationToken);

            if (user is null)
            {
                throw new ApplicationException("Authenticated user could not be found in database");
            }

            var hostUsername = activity.Attendees.FirstOrDefault(at => at.IsHost)?.AppUser?.UserName;

            var attendance = activity.Attendees.FirstOrDefault(at => at.AppUser?.UserName == user.UserName);

            if (attendance is not null && hostUsername == user.UserName)
            {
                activity.IsActive = !activity.IsActive;
            }

            if (attendance is not null && hostUsername != user.UserName)
            {
                activity.Attendees.Remove(attendance);
            }

            if (attendance is null)
            {
                attendance = new ActivityAttendee
                {
                    AppUserId = user.Id,
                    AppUser = user,
                    Activity = activity,
                    ActivityId = request.Id,
                    IsHost = false
                };

                activity.Attendees.Add(attendance);
            }

            var result = await _activityRepository.UpdateActivity(activity, cancellationToken);

            if (!result)
            {
                throw new ApplicationException("Attendance could not be updated");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
