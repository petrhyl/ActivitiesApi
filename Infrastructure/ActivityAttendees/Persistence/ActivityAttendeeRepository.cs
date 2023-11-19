using Application.Interfaces;
using Domain.Models;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ActivityAttendees.Persistence;

public class ActivityAttendeeRepository : IActivityAttendeeRepository
{
    private readonly DataContext _dataContext;

    public ActivityAttendeeRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<ActivityAttendee?> GetActivityAttendeeByUserId(Guid activityId, string userId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.ActivityAttendees.FindAsync(new object[] { userId, activityId }, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ActivityAttendee>> GetActivityAttendees(Guid activityId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.ActivityAttendees.Where(at => at.ActivityId == activityId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityAttendee>> GetUserActivityAttendees(string userId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.ActivityAttendees.Where(at => at.AppUserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateActivityAttendee(Guid activityId, string userId, CancellationToken cancellationToken = default)
    {
        var attendee = new ActivityAttendee
        {
            AppUserId = userId        ,
            ActivityId = activityId,
            IsHost = false
        };

        await _dataContext.ActivityAttendees.AddAsync(attendee, cancellationToken);

        return 0 > await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
