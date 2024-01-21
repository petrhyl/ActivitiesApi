using Domain.Models;

namespace Application.Repositories;

public interface IAppUserRepository
{
    Task<AppUser?> GetAppUserById(string id, CancellationToken cancellationToken = default);

    Task<bool> UpdateAppUser(AppUser user, CancellationToken cancellationToken = default);

    Task<bool> AddUserPhoto(string userId, PhotoImage photo, CancellationToken cancellationToken = default);

    Task<PhotoImage?> GetUserPhotoById(string photoId, string userId, CancellationToken cancellationToken = default);

    Task<ICollection<PhotoImage>> GetUserPhotos(string username, CancellationToken cancellationToken = default);

    Task<bool> DeleteUserPhoto(string photoId, string userId, CancellationToken cancellationToken = default);

    Task<bool> SetUserMainPhoto(string photoId, string userId, CancellationToken cancellationToken = default);

    Task<AppUser?> GetUserByUsername(string username, CancellationToken cancellationToken = default);

    Task<bool> UpdateFollowee(AppUser followee, AppUser follower, CancellationToken cancellationToken = default);

    Task<IEnumerable<AppUser>> GetUserFollowees(string username, CancellationToken cancellationToken = default);

    Task<IEnumerable<AppUser>> GetUserFollowers(string username, CancellationToken cancellationToken = default);
}

