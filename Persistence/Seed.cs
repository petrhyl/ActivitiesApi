using Domain.Models;

namespace Persistence;

public class Seed
{
    public static async Task SeedData(DataContext context)
    {
        if (context.Activities.Any())
        {
            return;
        }

        var activityCategories = new List<ActivityCategory>
        {
            new ActivityCategory
            {
                Name = "Drinks",
                Value = "drinks"
            },
            new ActivityCategory
            {
                Name = "Culture",
                Value = "culture"
            },
            new ActivityCategory
            {
                Name = "Music",
                Value = "music"
            },
            new ActivityCategory
            {
                Name = "Film",
                Value = "film"
            },
            new ActivityCategory
            {
                Name = "Food",
                Value = "food"
            },
            new ActivityCategory
            {
                Name = "Travel",
                Value = "travel"
            },
        };

        await context.ActivityCategories.AddRangeAsync(activityCategories);
        await context.SaveChangesAsync();

        var categories = context.ActivityCategories.ToArray();

        var activities = new List<Activity>
            {
                new Activity
                {
                    Title = "Past Activity 1",
                    BeginDate = DateTime.UtcNow.AddDays(14),
                    Description = "Activity 2 months ago",
                    CategoryId = categories.Where( c => c.Value == "drinks").First().Id!.Value,
                    City = "London",
                    Venue = "Pub",
                },
                new Activity
                {
                    Title = "Past Activity 2",
                    BeginDate = DateTime.UtcNow.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    CategoryId = categories.Where( c => c.Value == "culture").First().Id!.Value,
                    City = "Paris",
                    Venue = "Louvre",
                },
                new Activity
                {
                    Title = "Future Activity 1",
                    BeginDate = DateTime.UtcNow.AddDays(32),
                    Description = "Activity 1 month in future",
                    CategoryId = categories.Where( c => c.Value == "food").First().Id!.Value,
                    City = "London",
                    Venue = "Natural History Museum",
                },
                new Activity
                {
                    Title = "Future Activity 2",
                    BeginDate = DateTime.UtcNow.AddDays(32),
                    Description = "Activity 2 months in future",
                    CategoryId = categories.Where( c => c.Value == "music").First().Id!.Value,
                    City = "London",
                    Venue = "O2 Arena",
                },
                new Activity
                {
                    Title = "Future Activity 3",
                    BeginDate = DateTime.UtcNow.AddDays(23),
                    Description = "Activity 3 months in future",
                    CategoryId = categories.Where( c => c.Value == "culture").First().Id!.Value,
                    City = "London",
                    Venue = "Another pub",
                },
                new Activity
                {
                    Title = "Future Activity 4",
                    BeginDate = DateTime.UtcNow.AddDays(14),
                    Description = "Activity 4 months in future",
                    CategoryId = categories.Where( c => c.Value == "drinks").First().Id!.Value,
                    City = "London",
                    Venue = "Yet another pub",
                },
                new Activity
                {
                    Title = "Future Activity 5",
                    BeginDate = DateTime.UtcNow.AddDays(5),
                    Description = "Activity 5 months in future",
                    CategoryId = categories.Where( c => c.Value == "culture").First().Id!.Value,
                    City = "London",
                    Venue = "Just another pub",
                },
                new Activity
                {
                    Title = "Future Activity 6",
                    BeginDate = DateTime.UtcNow.AddMonths(3),
                    Description = "Activity 6 months in future",
                    CategoryId = categories.Where(c => c.Value == "music").First().Id !.Value,
                    City = "London",
                    Venue = "Roundhouse Camden",
                },
                new Activity
                {
                    Title = "Future Activity 7",
                    BeginDate = DateTime.UtcNow.AddMonths(2),
                    Description = "Activity 2 months ago",
                    CategoryId = categories.Where(c => c.Value == "film").First().Id !.Value,
                    City = "London",
                    Venue = "Somewhere on the Thames",
                },
                new Activity
                {
                    Title = "Future Activity 8",
                    BeginDate = DateTime.UtcNow.AddMonths(1),
                    Description = "Activity 8 months in future",
                    CategoryId = categories.Where(c => c.Value == "drinks").First().Id !.Value,
                    City = "London",
                    Venue = "Cinema",
                }
            };

        await context.Activities.AddRangeAsync(activities);
        await context.SaveChangesAsync();
    }
}
