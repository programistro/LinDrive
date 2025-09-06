using Atlant.Application.Interfaces;
using Atlant.Infractructure.Repositories;
using LinDrive.Application.Interfaces;
using LinDrive.Application.Services;
using LinDrive.Application.Services.IO.Interfaces;
using LinDrive.Application.Services.IO.Services;
using LinDrive.Core.Interfaces;
using LinDrive.Infrastructure.Repositories;

namespace LinDrive.Web;

public static class ConfigureBuilder
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMediaService, MediaService>();
    }
}