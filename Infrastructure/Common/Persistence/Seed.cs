using Domain.Models;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace Infrastructure.Common.Persistence;

public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
    {
        var users = new List<AppUser>();
        if (!userManager.Users.Any())
        {
            users = new List<AppUser>
            {
                new AppUser
                {
                    DisplayName = "Bob",
                    UserName = "bob",
                    Email = "bob@test.com"
                },
                new AppUser
                {
                    DisplayName = "Jane",
                    UserName = "jane",
                    Email = "jane@test.com"
                },
                new AppUser
                {
                    DisplayName = "Tom",
                    UserName = "tom",
                    Email = "tom@test.com"
                },
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$sw0rd");
            }
        }

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
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "bob@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "bob@test.com").First(),
                        IsHost = true
                    }
                }
            },
            new Activity
            {
                Title = "Past Activity 2",
                BeginDate = DateTime.UtcNow.AddMonths(-1),
                Description = "Activity 1 month ago",
                CategoryId = categories.Where( c => c.Value == "culture").First().Id!.Value,
                City = "Paris",
                Venue = "Louvre",
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "bob@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "bob@test.com").First(),
                        IsHost = true
                    },
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "jane@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "jane@test.com").First(),
                        IsHost = false
                    }
                }
            },
            new Activity
            {
                Title = "Future Activity 1",
                BeginDate = DateTime.UtcNow.AddDays(32),
                Description = "Activity 1 month in future",
                CategoryId = categories.Where( c => c.Value == "food").First().Id!.Value,
                City = "London",
                Venue = "Natural History Museum",
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "tom@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "tom@test.com").First(),
                        IsHost = true
                    },
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "jane@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "jane@test.com").First(),
                        IsHost = false
                    },
                }
            },
            new Activity
            {
                Title = "Future Activity 2",
                BeginDate = DateTime.UtcNow.AddDays(32),
                Description = "Activity 2 months in future",
                CategoryId = categories.Where( c => c.Value == "music").First().Id!.Value,
                City = "London",
                Venue = "O2 Arena",
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "bob@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "bob@test.com").First(),
                        IsHost = true
                    },
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "tom@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "tom@test.com").First(),
                        IsHost = false
                    },
                }
            },
            new Activity
            {
                Title = "Future Activity 3",
                BeginDate = DateTime.UtcNow.AddDays(23),
                Description = "Activity 3 months in future",
                CategoryId = categories.Where( c => c.Value == "culture").First().Id!.Value,
                City = "London",
                Venue = "Another pub",
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "jane@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "jane@test.com").First(),
                        IsHost = true
                    },
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "bob@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "bob@test.com").First(),
                        IsHost = false
                    },
                }
            },
            new Activity
            {
                Title = "Future Activity 5",
                BeginDate = DateTime.UtcNow.AddDays(5),
                Description = "Activity 5 months in future",
                CategoryId = categories.Where( c => c.Value == "culture").First().Id!.Value,
                City = "London",
                Venue = "Just another pub",
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "jane@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "jane@test.com").First(),
                        IsHost = true
                    }
                }
            },
            new Activity
            {
                Title = "Future Activity 6",
                BeginDate = DateTime.UtcNow.AddMonths(3),
                Description = "Activity 6 months in future",
                CategoryId = categories.Where(c => c.Value == "music").First().Id !.Value,
                City = "London",
                Venue = "Roundhouse Camden",
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "bob@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "bob@test.com").First(),
                        IsHost = true
                    },
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "jane@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "jane@test.com").First(),
                        IsHost = false
                    },
                }
            },
            new Activity
            {
                Title = "Future Activity 7",
                BeginDate = DateTime.UtcNow.AddMonths(2),
                Description = "Activity 2 months ago",
                CategoryId = categories.Where(c => c.Value == "film").First().Id !.Value,
                City = "London",
                Venue = "Somewhere on the Thames",
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "tom@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "tom@test.com").First(),
                        IsHost = true
                    },
                    new ActivityAttendee
                    {
                        AppUserId = userManager.Users.Where(u => u.Email == "jane@test.com").First().Id,
                        AppUser = userManager.Users.Where(u => u.Email == "jane@test.com").First(),
                        IsHost = false
                    },
                }
            },
            new Activity
            {
                Title = "Future Activity 8",
                BeginDate = DateTime.UtcNow.AddMonths(1),
                Description = "Activity 8 months in future",
                CategoryId = categories.Where(c => c.Value == "drinks").First().Id !.Value,
                City = "London",
                Venue = "Cinema",
                    Attendees = new List<ActivityAttendee>
                    {
                        new ActivityAttendee
                        {
                            AppUserId = userManager.Users.Where(u => u.Email == "bob@test.com").First().Id,
                            AppUser = userManager.Users.Where(u => u.Email == "bob@test.com").First(),
                            IsHost = true
                        },
                        new ActivityAttendee
                        {
                            AppUserId = userManager.Users.Where(u => u.Email == "tom@test.com").First().Id,
                            AppUser = userManager.Users.Where(u => u.Email == "tom@test.com").First(),
                            IsHost = false
                        },
                    }
            }
        };

        await context.Activities.AddRangeAsync(activities);
        await context.SaveChangesAsync();
    }
}
