using LinDrive.Application.IO.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LinDrive.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddSingleton<FileWatcherService>();
        
        return services;
    }
}