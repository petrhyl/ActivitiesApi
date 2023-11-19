using Application.Activities.Validator;
using Domain.Core;
using Application.Mapping;
using Contracts.Request;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Application.Services.Auth;
using System.Security.Authentication;
using Application.Interfaces;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public required ActivityRequest Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IActivityRepository activityRepository)
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator(activityRepository));
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IActivityRepository _actvityRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAuthService _authService;

        public Handler(IActivityRepository activityRepository, IAuthService authService, IAppUserRepository appUserRepository)
        {
            _actvityRepository = activityRepository;
            _authService = authService;
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUserId = _authService.GetCurrentUserId();

            if (currentUserId is null)
            {
                throw new AuthenticationException("User is not authenticated.");
            }

            var user = await _appUserRepository.GetAppUserById(currentUserId, cancellationToken);

            if (user is null)
            {
                throw new ApplicationException("Authenticated user could not be found in database");
            }

            var activity = request.Activity.MapToActivityWithHost(user);

            var result = await _actvityRepository.CreateActivity(activity, cancellationToken);

            if (!result)
            {
                throw new ApplicationException("Creation of activity failed.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
