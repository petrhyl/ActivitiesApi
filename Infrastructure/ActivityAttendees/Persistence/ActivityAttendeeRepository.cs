using Application.Repositories;
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

    public async Task<ICollection<ActivityAttendee>> GetActivityAttendees(Guid activityId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.ActivityAttendees
            .Where(at => at.ActivityId == activityId)
            .Include(at => at.AppUser)            
                .ThenInclude(u => u.Followers)
                    .ThenInclude(f => f.Follower)
            .AsSplitQuery()
            .Include(at => at.AppUser)
                .ThenInclude(u => u.Followings)
                    .ThenInclude(f => f.Followee)
            .AsSplitQuery()
            .Include(at => at.AppUser)
                    .ThenInclude(u => u.Photos.Where(p => p.IsMain))
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<ActivityAttendee>> GetUserActivityAttendees(string userId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.ActivityAttendees.Where(at => at.AppUserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<bool> AddAttendee(Guid activityId, AppUser appUser, CancellationToken cancellationToken = default)
    {
        var attendee = new ActivityAttendee
        {
            AppUserId = appUser.Id,
            AppUser = appUser,
            ActivityId = activityId,
            IsHost = false
        };

        await _dataContext.ActivityAttendees.AddAsync(attendee, cancellationToken);

        return 0 > await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
