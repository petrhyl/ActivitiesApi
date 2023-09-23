using Application.Activities.Validator;
using Application.Core;
using Application.Mapping;
using Application.Request;
using Domain.Models;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public required ActivityRequest Activity { get; set; }
    }

    public class CommandValidator: AbstractValidator<Command>
    { 
        public CommandValidator(DataContext dataContext)
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator(dataContext));
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _dataContext;

        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = request.Activity.MapToActivity();

            _dataContext.Activities.Add(activity);

            var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to create activity.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
