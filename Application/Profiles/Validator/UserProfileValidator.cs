using Application.Repositories;
using Application.Services.Auth;
using Contracts.Request;
using Contracts.Response;
using FluentValidation;

namespace Application.Profiles.Validator;

public class UserProfileValidator : AbstractValidator<ProfileRequest>
{

    public UserProfileValidator(IAuthService authService)
    {
        RuleFor(u => u.Username).Equal(authService.GetCurrentUser("").Result.Value!.Username);
        RuleFor(u => u.DisplayName).MinimumLength(3).MaximumLength(50).WithMessage("Name's length must be between 3 an 50 characters.");
        RuleFor(u => u.Bio).MaximumLength(500).WithMessage("Maximum length of a text exceeded.");
    }
}
