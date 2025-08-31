using Microsoft.Extensions.DependencyInjection;

namespace LinDrive.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<FileSystemWatcher>();
    }
}