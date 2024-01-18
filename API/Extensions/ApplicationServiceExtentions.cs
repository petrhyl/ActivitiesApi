using API.SignalR.Providers;
using Application.Activities;
using Application.ChatPosts.Providers;
using Application.Services.ImageCloud;
using CloudinaryDotNet;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Extensions;

public static class ApplicationServiceExtentions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000");
            });
        });

        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblyContaining(typeof(ActivityList));
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Create>();
        services.AddHttpContextAccessor();

        services.Configure<Account>(config.GetSection("Cloudinary"));
        services.AddScoped<ICloudinary>(opt => new Cloudinary(opt.GetService<IOptions<Account>>()?.Value));
        services.AddScoped<IImageCloudService, ImageCloudService>();
        services.AddScoped<IHubContextProvider, HubContextProvider>();
        services.AddSignalR();

        return services;
    }
}
