using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Activity
{
    public Guid? Id { get; set; }

    public required string Title { get; set; }

    public required DateTime BeginDate { get; set; }

    public string? Description { get; set; }

    [ForeignKey(nameof(ActivityCategory))]
    public required Guid CategoryId { get; set; }

    public ActivityCategory? ActivityCategory { get; set; }

    public required string City { get; set; }

    public required string Venue { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();
}
