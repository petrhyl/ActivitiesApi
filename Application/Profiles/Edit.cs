using Application.Activities.Validator;
using Application.Repositories;
using Application.Profiles.Validator;
using Application.Services.Auth;
using Contracts.Request;
using Domain.Core;
using FluentValidation;
using MediatR;

namespace Application.Profiles;

public class Edit
{
    public record Command(ProfileRequest UserProfile) : IRequest<Result<Unit>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IAuthService authService)
        {
            RuleFor(r => r.UserProfile).SetValidator(new UserProfileValidator(authService));
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAuthService _authService;

        public Handler(IAuthService authService, IAppUserRepository userRepository)
        {
            _authService = authService;
            _appUserRepository = userRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _appUserRepository.GetAppUserById(_authService.GetCurrentUserId()!);

            if (user is null)
            {
                throw new ApplicationException("Cannot find user in database");
            }

            if (user.UserName != request.UserProfile.Username)
            {
                return Result<Unit>.Failure("The edited user profile does not belong to the authenticated user.");
            }

            user.DisplayName = request.UserProfile.DisplayName;
            user.Bio = request.UserProfile.Bio;

            var result = await _appUserRepository.UpdateAppUser(user, cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
