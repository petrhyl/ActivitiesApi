using Domain.Models;

namespace Application.Interfaces;

public interface IAppUserRepository
{
    Task<AppUser?> GetAppUserById(string id, CancellationToken cancellationToken = default);    
}

