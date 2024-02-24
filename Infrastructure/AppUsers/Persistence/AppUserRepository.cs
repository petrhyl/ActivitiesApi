using Application.Repositories;
using Domain.Models;
using Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

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
            .AsSplitQuery()
            .Include(us => us.Followees)
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
            return new Collection<PhotoImage>();
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
            return new Collection<PhotoImage>();
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
            .AsSplitQuery()
            .Include(us => us.Followees)
            .SingleOrDefaultAsync(u => u.UserName == username, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateFollowing(AppUser followee, AppUser follower, CancellationToken cancellationToken = default)
    {
        if (followee.Followers.Any(fr => fr.Id == follower.Id))
        {
            followee.Followers.Remove(follower);
        }
        else
        {
            followee.Followers.Add(follower);
        }

        var result = await _dataContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<IEnumerable<AppUser>> GetUserFollowees(string username, CancellationToken cancellationToken = default)
    {
        var user = await _dataContext.Users
            .AsNoTracking()
            .Include(u => u.Followees)
                .ThenInclude(fe => fe.Photos.Where(p => p.IsMain))
            .AsSplitQuery()
            .Include(u => u.Followees)
                .ThenInclude(fr => fr.Followers)
            .AsSplitQuery()
            .Include(u => u.Followees)
                .ThenInclude(fr => fr.Followees)
            .FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);

        if (user is null)
        {
            return new List<AppUser>();
        }

        return user.Followees;
    }

    public async Task<IEnumerable<AppUser>> GetUserFollowers(string username, CancellationToken cancellationToken = default)
    {
        var user = await _dataContext.Users
            .AsNoTracking()
            .Include(u => u.Followers)
               .ThenInclude(fr => fr.Photos.Where(p => p.IsMain))
            .AsSplitQuery()
            .Include(u => u.Followers)
                .ThenInclude(fr => fr.Followers)
            .AsSplitQuery()
            .Include(u => u.Followers)
                .ThenInclude(fr => fr.Followees)
            .FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);

        if (user is null)
        {
            return new List<AppUser>();
        }

        return user.Followers;
    }

    public async Task<bool> RemoveFollower(string currentUserId, string followerUsername, CancellationToken cancellationToken = default)
    {
        var follower = await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName == followerUsername, cancellationToken);

        if (follower is null)
        {
            return false;
        }

        var followee = await _dataContext.Users
            .Include(u => u.Followers)
            .FirstOrDefaultAsync(f => f.Id == currentUserId, cancellationToken);

        if (followee is null)
        {
            return false;
        }

        followee.Followers.Remove(follower);

        var result = await _dataContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<bool> DoesUserExistWithUsername(string username, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken) is not null;
    }

    public async Task<bool> DoesUserExistWithId(string id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Users.FindAsync(new object[] { id }, cancellationToken: cancellationToken) is not null;
    }
}
