using Application.Repositories;
using Domain.Models;
using Infrastructure.Common.Persistence;
using MediatR;
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
        return await _dataContext.Users
            .Include(u => u.Photos.Where(p => p.IsMain))
            .AsSplitQuery()
            .Include(us => us.Followers)
                .ThenInclude(uf => uf.Follower)
            .Include(us => us.Followings)
                .ThenInclude(uf => uf.Followee)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAppUser(AppUser appUser, CancellationToken cancellationToken = default)
    {
        var user = await GetAppUserById(appUser.Id, cancellationToken);

        if (user is null)
        {
            throw new ApplicationException("User was not found by ID");
        }

        user.DisplayName = appUser.DisplayName;
        user.Email = appUser.Email;
        user.Bio = appUser.Bio;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return true;
    }


    public async Task<bool> AddUserPhoto(string userId, PhotoImage photo, CancellationToken cancellationToken = default)
    {
        var user = await GetAppUserById(userId, cancellationToken);

        if (user is null)
        {
            return false;
        }

        PhotoImage? previousMainPhoto = null;

        if (photo.IsMain)
        {
            var photos = await GetUserPhotos(user.UserName!, cancellationToken);
            previousMainPhoto = photos.FirstOrDefault(p => p.IsMain);
        }

        user.Photos.Add(photo);

        if (previousMainPhoto is not null)
        {
            previousMainPhoto.IsMain = false;
        }

        var result = await _dataContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<PhotoImage?> GetUserPhotoById(string photoId, string userId, CancellationToken cancellationToken = default)
    {
        return await _dataContext.PhotoImages.FirstOrDefaultAsync(p => p.Id == photoId && p.AppUserId == userId, cancellationToken: cancellationToken);
    }

    public async Task<ICollection<PhotoImage>> GetUserPhotos(string username, CancellationToken cancellationToken = default)
    {
        var user = await _dataContext.Users
            .Include(u => u.Photos)
            .SingleOrDefaultAsync(u => u.UserName == username, cancellationToken: cancellationToken);

        if (user is null)
        {
            throw new ArgumentException("User not found", nameof(username));
        }

        return user.Photos;
    }

    public async Task<ICollection<PhotoImage>> GetUserPhotosByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _dataContext.Users
            .Include(u => u.Photos)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);

        if (user is null)
        {
            throw new ArgumentException("User not found by ID.");
        }

        return user.Photos;
    }

    public async Task<bool> DeleteUserPhoto(string photoId, string userId, CancellationToken cancellationToken = default)
    {
        var photos = await GetUserPhotosByUserId(userId, cancellationToken);

        var photo = photos.FirstOrDefault(p => p.Id == photoId);

        if (photo is null)
        {
            return false;
        }

        photos.Remove(photo);

        var result = await _dataContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<bool> SetUserMainPhoto(string photoId, string userId, CancellationToken cancellationToken = default)
    {
        var photos = await GetUserPhotosByUserId(userId, cancellationToken);

        var previousMainPhoto = photos.FirstOrDefault(p => p.IsMain);

        var photo = photos.FirstOrDefault(p => p.Id == photoId);

        if (photo is null)
        {
            return false;
        }

        if (previousMainPhoto is not null)
        {
            if (previousMainPhoto.Id == photo.Id)
            {
                return true;
            }

            previousMainPhoto.IsMain = false;
        }

        photo.IsMain = true;

        var result = await _dataContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<AppUser?> GetUserByUsername(string username, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Users
            .Include(u => u.Photos.Where(p => p.IsMain))
            .AsSplitQuery()
            .Include(us => us.Followers)
            .ThenInclude(uf => uf.Follower)
            .Include(us => us.Followings)
            .ThenInclude(uf => uf.Followee)
            .SingleOrDefaultAsync(u => u.UserName == username, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateFollowee(AppUser followee, AppUser follower, CancellationToken cancellationToken = default)
    {
        var following = await _dataContext.UserFollowings.FindAsync(follower.Id, followee.Id, cancellationToken);

        if (following is null)
        {
            following = new UserFollowing
            {
                Follower = follower,
                FollowerId = follower.Id,
                Followee = followee,
                FolloweeId = followee.Id
            };

            _dataContext.UserFollowings.Add(following);
        }
        else
        {
            _dataContext.UserFollowings.Remove(following);
        }

        var result = await _dataContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<IEnumerable<AppUser>> GetUserFollowees(string username, CancellationToken cancellationToken = default)
    {
        return await _dataContext.UserFollowings  
            .Where(f => f.Follower.UserName == username)
            .Select(f => f.Followee)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppUser>> GetUserFollowers(string username, CancellationToken cancellationToken = default)
    {
        return await _dataContext.UserFollowings
            .Where(f => f.Followee.UserName == username)
            .Select(f => f.Follower)
            .ToListAsync(cancellationToken);
    }
}
