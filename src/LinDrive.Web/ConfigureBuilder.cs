using LinDrive.Application.Interfaces;
using LinDrive.Application.Services;
using LinDrive.Application.Services.IO.Interfaces;
using LinDrive.Application.Services.IO.Services;
using LinDrive.Core.Interfaces;
using LinDrive.Infrastructure.Repositories;

namespace LinDrive.Web;

public static class ConfigureBuilder
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMediaService, MediaService>();

        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();

        return services;
    }
    
    public static IServiceCollection ConfigureValidatos(this IServiceCollection services)
    {
        return services;
    }
}