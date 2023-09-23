using Application.Request;
using FluentValidation;
using Persistence;

namespace Application.Activities.Validator;

public class ActivityValidator : AbstractValidator<ActivityRequest>
{
    private readonly DataContext _dataContext;

    public ActivityValidator(DataContext dataContext)
    {
        _dataContext = dataContext;

        RuleFor(a => a.Title).NotEmpty();
        RuleFor(a => a.Category.Id).NotEmpty().Must(IsCategoryIdValid).WithMessage("Invalid ID of the activity's category");
        RuleFor(a => a.BeginDate).NotEmpty().Must(d => d != DateTime.MinValue).WithMessage("Invalid date of the Activity.");
        RuleFor(a => a.City).NotEmpty();
        RuleFor(a => a.Venue).NotEmpty();
    }

    private bool IsCategoryIdValid(Guid categoryId)
    {
        var category = _dataContext.ActivityCategories.Find(new object[] { categoryId });

        return category is not null;
    }
}
