using Domain.Models;

namespace Application.Interfaces;

public interface IAppUserRepository
{
    Task<AppUser?> GetAppUserById(string id, CancellationToken cancellationToken = default);

    Task<bool> AddUserPhoto(string userId, PhotoImage photo, CancellationToken cancellationToken = default);

    Task<PhotoImage?> GetUserPhotoById(string photoId, string userId, CancellationToken cancellationToken = default);

    Task<ICollection<PhotoImage>> GetUserPhotos(string username, CancellationToken cancellationToken = default);

    Task<bool> DeleteUserPhoto(string photoId, string userId, CancellationToken cancellationToken = default);

    Task<bool> SetUserMainPhoto(string photoId, string userId, CancellationToken cancellationToken = default);

    Task<AppUser?> GetUserByUsername(string username, CancellationToken cancellationToken = default);
}

