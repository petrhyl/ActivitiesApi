using Domain.Models;

namespace Application.Interfaces;

public interface IActivityRepository
{
    Task<IEnumerable<Activity>> GetActivities(CancellationToken cancellationToken = default);

    Task<Activity?> GetActivityById(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<ActivityCategory>> GetActivityCategories(CancellationToken cancellationToken = default);

    Task<bool> CreateActivity(Activity activity, CancellationToken cancellationToken = default);

    Task<bool> UpdateActivity(Activity activity, CancellationToken cancellationToken = default);

    Task<bool> DeleteActivity(Guid id, CancellationToken cancellationToken = default);

    Task<bool> CreateChatPost(Activity activity, ChatPost chatPost, CancellationToken cancellationToken = default);

    Task<IEnumerable<ChatPost>> GetChatPostsOfActivity(Guid activityId, CancellationToken cancellationToken = default);
}

