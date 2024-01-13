using Application.Interfaces;
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
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAppUser(AppUser appUser, CancellationToken cancellationToken = default) { 
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
            .SingleOrDefaultAsync(u => u.UserName == username, cancellationToken: cancellationToken);
    }
}
