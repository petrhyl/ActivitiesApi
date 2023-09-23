using Domain.Models;
using Application.Request;
using Application.Response;
using Application.TransferObjects.Response;

namespace Application.Mapping;

public static class ActivityMapper
{
    public static ActivityResponse MapToResponse(this Activity activity, ActivityCategory category)
    {
        return new ActivityResponse
        {
            Id = activity.Id.Value,
            Title = activity.Title,
            Description = activity.Description,
            Category = new ActivityCategoryResponse
            {
                Id = category.Id!.Value,
                Value = category.Value,
                Name = category.Name
            },
            BeginDate = activity.BeginDate,
            City = activity.City,
            Venue = activity.Venue,
        };
    }

    public static void ModifyActivity(this Activity activity, ActivityRequest request)
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
        };
    }

    public static IEnumerable<ActivityResponse> MapToResponse(this IEnumerable<Activity> activities, IEnumerable<ActivityCategory> categories)
    {
        return activities.Select(a =>
        {
            var category = categories.Where(c => c.Id == a.CategoryId).First();

            return new ActivityResponse
            {
                Id = a.Id!.Value,
                Title = a.Title,
                BeginDate = a.BeginDate,
                Description = a.Description,
                Category = new ActivityCategoryResponse
                {
                    Id = a.CategoryId,
                    Name = category.Name,
                    Value = category.Value,
                },
                City = a.City,
                Venue = a.Venue,
            };
        });
    }
}

