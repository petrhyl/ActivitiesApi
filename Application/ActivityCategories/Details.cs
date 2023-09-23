using Application.Core;
using Application.Mapping;
using Application.TransferObjects.Response;
using MediatR;
using Persistence;

namespace Application.ActivityCategories;

public class Details
{
    public class Query : IRequest<Result<ActivityCategoryResponse>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ActivityCategoryResponse>>
    {
        private readonly DataContext _dataContext;

        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Result<ActivityCategoryResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var category = await _dataContext.ActivityCategories.FindAsync(new object[] { request.Id }, cancellationToken);

            if (category is null)
            {
                throw new ArgumentException("The category does not exist");
            }

            var response = category.MapToResponse();

            return Result<ActivityCategoryResponse>.Success(response);
        }
    }
}

