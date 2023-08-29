using Domain.Models;

namespace Application.Mapping;

public static class ActivityMapper
{
    public static Activity MapToActivity(this Activity activity, Activity mappingActivity)
    {
        activity.Id = mappingActivity.Id;
        activity.Title = mappingActivity.Title;
        activity.Description = mappingActivity.Description;
        activity.Category = mappingActivity.Category;
        activity.BeginDate = mappingActivity.BeginDate;
        activity.City = mappingActivity.City;
        activity.Venue = mappingActivity.Venue;

        return activity;
    }

    public static Activity MapToNewActivity(this Activity activity)
    {
        return new Activity
        {
            Title = activity.Title,
            Description = activity.Description,
            Category = activity.Category,
            BeginDate = activity.BeginDate,
            City = activity.City,
            Venue = activity.Venue,
        };
    }
}

