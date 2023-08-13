using MediatR;
using Persistence;

namespace Application.Activities;

public class Delete
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _dataContext;

        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _dataContext.Activities.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            _dataContext.Remove(activity);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
