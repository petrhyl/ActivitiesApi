using Application.Request;
using FluentValidation;

namespace Application.Activities;
public class ActivityValidator: AbstractValidator<ActivityRequest>
{
    public ActivityValidator()
    {
        RuleFor(a => a.Title).NotEmpty();
        RuleFor(a => a.CategoryId).NotEmpty().Must( a => Guid.TryParse(a.ToString(), out _)).WithMessage("Invalid ID of the activity's category");
        RuleFor(a => a.BeginDate).NotEmpty().Must(d => d != DateTime.MinValue).WithMessage("Invalid date of the Activity.");
        RuleFor(a => a.City).NotEmpty();
        RuleFor(a => a.Venue).NotEmpty();
    }
}
