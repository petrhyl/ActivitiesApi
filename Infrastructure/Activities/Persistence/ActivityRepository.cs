using Application.Interfaces;
using Domain.Models;
using Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Activities.Persistence;

public class ActivityRepository : IActivityRepository
{
    private readonly DataContext _dataContext;

    public ActivityRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task<IEnumerable<Activity>> GetActivities(CancellationToken cancellationToken = default)
    {
        return await _dataContext.Activities
            .Include(a => a.ActivityCategory)
            .Include(a => a.Attendees)
            .ThenInclude(at => at.AppUser)
            .ThenInclude(u => u.Photos.Where(p => p.IsMain))
            .ToListAsync(cancellationToken);
    }

    public Task<Activity?> GetActivityById(Guid id, CancellationToken cancellationToken = default)
    {
        return _dataContext.Activities
            .Include(a => a.ActivityCategory)
            .Include(a => a.Attendees)
            .ThenInclude(at => at.AppUser)
            .ThenInclude(u => u.Photos.Where(p => p.IsMain))
            .SingleOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> CreateActivity(Activity activity, CancellationToken cancellationToken = default)
    {
        await _dataContext.Activities.AddAsync(activity, cancellationToken);

        return 0 < await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateActivity(Activity activity, CancellationToken cancellationToken = default)
    {
        var previousActivity = await GetActivityById(activity.Id!.Value, cancellationToken);

        if (previousActivity is null)
        {
            return false;
        }

        previousActivity.Title = activity.Title;
        previousActivity.Description = activity.Description;
        previousActivity.CategoryId = activity.CategoryId;
        previousActivity.BeginDate = activity.BeginDate;
        previousActivity.City = activity.City;
        previousActivity.Venue = activity.Venue;

        return 0 < await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteActivity(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await GetActivityById(id, cancellationToken);

        if (activity is null)
        {
            return false;
        }

        _dataContext.Activities.Remove(activity);

        return 0 < await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityCategory>> GetActivityCategories(CancellationToken cancellationToken = default)
    {
        return await _dataContext.ActivityCategories.ToListAsync(cancellationToken);
    }

    public async Task<bool> CreateChatPost(Activity activity, ChatPost chatPost, CancellationToken cancellationToken = default)
    {
        activity.Posts.Add(chatPost);

        var result = await _dataContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<IEnumerable<ChatPost>> GetChatPostsOfActivity(Guid activityId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.ChatPosts
            .Where(p => p.Activity.Id == activityId)
            .Include(p => p.Author)
            .ThenInclude(a => a.Photos.Where(m => m.IsMain))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
