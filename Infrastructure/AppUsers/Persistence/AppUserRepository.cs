using Application.Interfaces;
using Domain.Models;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AppUsers.Persistence;

public class AppUserRepository : IAppUserRepository
{
    private readonly DataContext _dataContext;

    public AppUserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<AppUser?> GetAppUserById(string id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Users.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }
}
