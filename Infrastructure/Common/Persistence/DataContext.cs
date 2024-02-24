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


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<ActivityAttendee>(attendee =>
        {
            attendee.HasKey(a => new { a.AppUserId, a.ActivityId });

            attendee
                .HasOne(at => at.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(at => at.ActivityId);

            attendee
                .HasOne(at => at.AppUser)
                .WithMany(u => u.Attendees)
                .HasForeignKey(at => at.AppUserId);
        });

        builder.Entity<ChatPost>(post =>
        {
            post
            .HasOne(c => c.Activity)
            .WithMany(a => a.Posts)
            .OnDelete(DeleteBehavior.Cascade);
        });


        builder.Entity<AppUser>(user =>
        {
            user
                .HasMany(u => u.Followees)
                .WithMany(u => u.Followers)
                .UsingEntity("Followee_Follower",
                    left => left
                        .HasOne(typeof(AppUser))
                        .WithMany()
                        .HasForeignKey("FolloweeId")
                        .HasPrincipalKey(nameof(AppUser.Id))
                        .HasConstraintName("FK_FolloweeFollower_Followee")
                        .OnDelete(DeleteBehavior.Cascade),
                    right => right
                        .HasOne(typeof(AppUser))
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .HasPrincipalKey(nameof(AppUser.Id))
                        .HasConstraintName("FK_FolloweeFollower_Follower")
                        .OnDelete(DeleteBehavior.Cascade),
                    linkBuilder => linkBuilder.HasKey("FolloweeId", "FollowerId"));

            user
                .Property(u => u.UserName)
                .IsRequired();

            user
                .HasIndex(u => u.UserName, "AK_User_Username")
                .IsUnique();

        });
    }
}
