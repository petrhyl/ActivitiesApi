using Domain.Models;

namespace Application.Repositories;

public interface IActivityAttendeeRepository
{
    Task<ICollection<ActivityAttendee>> GetActivityAttendees(Guid activityId, CancellationToken cancellationToken = default);

    Task<ActivityAttendee?> GetActivityAttendeeByUserId(Guid activityId, string userId, CancellationToken cancellationToken = default);

    Task<ICollection<ActivityAttendee>> GetUserActivityAttendees(string userId, CancellationToken cancellationToken = default);

    Task<bool> AddAttendee(Guid activityId, AppUser appUser, CancellationToken cancellationToken = default);
}

