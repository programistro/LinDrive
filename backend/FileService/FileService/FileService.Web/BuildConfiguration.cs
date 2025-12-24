using FileService.Appliation.Services.IO;
using LinDrive.Application.Services.IO.Interfaces;

namespace FileService.Web;

public static class BuildConfiguration
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureServices()
        {
            services.AddScoped<IMediaService, MediaService>();
            return services;
        }
    }
}