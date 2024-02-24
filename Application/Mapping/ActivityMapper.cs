using Domain.Models;
using Contracts.Request;
using Contracts.Response;

namespace Application.Mapping;

public static class ActivityMapper
{
    public static ActivityResponse MapToResponse(this Activity activity, string? currentUsername)
    {
        var attenders = activity.Attendees.MapToResponse(currentUsername);

        return new ActivityResponse
        {
            Id = activity.Id!.Value,
            Title = activity.Title,
            Description = activity.Description,
            Category = new ActivityCategoryResponse
            {
                Id = activity.ActivityCategory!.Id!.Value,
                Value = activity.ActivityCategory!.Value!,
                Name = activity.ActivityCategory!.Name!
            },
            BeginDate = activity.BeginDate,
            City = activity.City,
            Venue = activity.Venue,
            IsActive = activity.IsActive,
            Attenders = attenders,
            Host = activity.Host.AppUser.MapToProfileWithoutFollowing()
        };
    }

    public static void ModifyActivity(this Activity activity, ActivityRequest request, IEnumerable<AppUser> attenders)
    {
        activity.Title = request.Title;
        activity.Description = request.Description;
        activity.CategoryId = request.Category.Id;
        activity.BeginDate = request.BeginDate;
        activity.City = request.City;
        activity.Venue = request.Venue;
    }

    public static Activity MapToActivity(this ActivityRequest request)
    {
        return new Activity
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            CategoryId = request.Category.Id,
            BeginDate = request.BeginDate,
            City = request.City,
            Venue = request.Venue,
            IsActive = request.IsActive,
        };
    }

    public static Activity MapToActivityWithHost(this ActivityRequest request, AppUser attender)
    {
        var activity = request.MapToActivity();

        activity.Attendees.Add(new ActivityAttendee
        {
            Activity = activity,
            ActivityId = activity.Id!.Value,
            AppUserId = attender.Id,
            AppUser = attender,
            IsHost = true
        });

        return activity;
    }

    public static IEnumerable<ActivityResponse> MapToResponse(this IEnumerable<Activity> activities, string? currentUsername)
    {
        return activities.Select(a => a.MapToResponse(currentUsername));
    }

    public static IEnumerable<ActivityAttenderResponse> MapToResponse(this ICollection<ActivityAttendee> attendees, string? currentUsername)
    {
        return attendees.Select(at =>
        {
            var attender = at.AppUser.MapToProfileWithoutFollowing();
            attender.IsFollowedByCurrentUser = at.AppUser.Followers.Any(fr => fr.UserName == currentUsername);
            attender.IsFollowingCurrentUser = at.AppUser.Followees.Any(fe => fe.UserName == currentUsername);

            return new ActivityAttenderResponse
            {
                Attender = attender,
                IsHost = at.IsHost
            };
        });
    }
}

