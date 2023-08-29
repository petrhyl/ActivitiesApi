using Domain.Models;
using FluentValidation;

namespace Application.Activities;
public class ActivityValidator: AbstractValidator<Activity>
{
    public ActivityValidator()
    {
        RuleFor(a => a.Title).NotEmpty();
        RuleFor(a => a.Category).NotEmpty();
        RuleFor(a => a.BeginDate).NotEmpty().Must(CheckActivityDate).WithMessage("Invalid date of the Activity.");
        RuleFor(a => a.City).NotEmpty();
        RuleFor(a => a.Venue).NotEmpty();
    }

    private bool CheckActivityDate(Activity activity, DateTime date)
    {
        if (date == DateTime.MinValue)
        {
            return false;
        }

        return true;
    }
}
