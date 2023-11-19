using Contracts.Response;
using Domain.Models;

namespace Application.Mapping;

public static class ActivityCategoryMapper
{
    public static ActivityCategoryResponse MapToResponse(this ActivityCategory category)
    {
        return new ActivityCategoryResponse
        {
            Id = category.Id!.Value,
            Value = category.Value,
            Name = category.Name
        };
    }

    public static IEnumerable<ActivityCategoryResponse> MapToResponse(this IEnumerable<ActivityCategory> categories)
    {
        return categories.Select(c => new ActivityCategoryResponse
        {
            Id = c.Id!.Value,
            Value = c.Value,
            Name = c.Name
        });
    }
}

