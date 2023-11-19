using Domain.Models;

namespace Application.Interfaces;

public interface IActivityAttendeeRepository
{
    Task<IEnumerable<ActivityAttendee>> GetActivityAttendees(Guid activityId, CancellationToken cancellationToken = default);

    Task<ActivityAttendee?> GetActivityAttendeeByUserId(Guid activityId, string userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<ActivityAttendee>> GetUserActivityAttendees(string userId, CancellationToken cancellationToken = default);

    Task<bool> UpdateActivityAttendee(Guid activityId, string userId, CancellationToken cancellationToken = default);
}

