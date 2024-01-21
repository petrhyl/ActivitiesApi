using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Activity> Activities { get; set; }

    public DbSet<ActivityCategory> ActivityCategories { get; set; }

    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }

    public DbSet<PhotoImage> PhotoImages { get; set; }

    public DbSet<ChatPost> ChatPosts { get; set; }

    public DbSet<UserFollowing> UserFollowings { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ActivityAttendee>(x => x.HasKey(a => new { a.AppUserId, a.ActivityId }));

        builder.Entity<ActivityAttendee>()
            .HasOne(at => at.AppUser)
            .WithMany(u => u.Attendees)
            .HasForeignKey(at => at.AppUserId);

        builder.Entity<ActivityAttendee>()
            .HasOne(at => at.Activity)
            .WithMany(a => a.Attendees)
            .HasForeignKey(at => at.ActivityId);

        builder.Entity<ChatPost>()
            .HasOne(c => c.Activity)
            .WithMany(a => a.Posts)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserFollowing>(t =>
        {
            t.HasKey(f => new { f.FollowerId, f.FolloweeId });

            t.HasOne(f => f.Follower)
                .WithMany(r => r.Followings)
                .HasForeignKey(r => r.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);

            t.HasOne(f => f.Followee)
                .WithMany(r => r.Followers)
                .HasForeignKey(r => r.FolloweeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
