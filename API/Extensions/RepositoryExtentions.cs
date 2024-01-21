using Application.Repositories;
using Infrastructure.Activities.Persistence;
using Infrastructure.ActivityAttendees.Persistence;
using Infrastructure.AppUsers.Persistence;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API.Extensions;

public static class RepositoryExtentions
{
    public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IActivityAttendeeRepository, ActivityAttendeeRepository>();

        return services;
    }
}
