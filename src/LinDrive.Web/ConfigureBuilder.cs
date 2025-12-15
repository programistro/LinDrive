using LinDrive.Application.Interfaces;
using LinDrive.Application.Services;
using LinDrive.Application.Services.IO.Interfaces;
using LinDrive.Application.Services.IO.Services;
using LinDrive.Core.Interfaces;
using LinDrive.Infrastructure.Repositories;
using LinDrive.Shared.Interfaces;
using LinDrive.Shared.Services;

namespace LinDrive.Web;

public static class ConfigureBuilder
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IInfoService, InfoService>();

        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
    
    public static IServiceCollection ConfigureValidatos(this IServiceCollection services)
    {
        return services;
    }
}