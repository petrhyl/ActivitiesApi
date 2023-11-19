using Application.Interfaces;
using Contracts.Request;
using FluentValidation;

namespace Application.Activities.Validator;

public class ActivityValidator : AbstractValidator<ActivityRequest>
{
    private readonly IActivityRepository _activityRepository;

    public ActivityValidator(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;

        RuleFor(a => a.Title).NotEmpty();
        RuleFor(a => a.Category.Id).NotEmpty().Must(IsCategoryIdValid).WithMessage("Invalid ID of the activity's category");
        RuleFor(a => a.BeginDate).NotEmpty().Must(d => d != DateTime.MinValue).WithMessage("Invalid date of the Activity.");
        RuleFor(a => a.City).NotEmpty();
        RuleFor(a => a.Venue).NotEmpty();
    }

    private bool IsCategoryIdValid(Guid categoryId)
    {
        var categories = _activityRepository.GetActivityCategories().Result;

        var result = categories.FirstOrDefault(c => c.Id == categoryId);

        return result is not null;
    }
}
