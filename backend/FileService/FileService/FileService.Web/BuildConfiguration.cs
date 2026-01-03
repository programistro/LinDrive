using FileService.Appliation.Services.IO;

namespace FileService.Web;

public static class BuildConfiguration
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureServices()
        {
            services.AddScoped<IS3Service, S3Service>();
            services.AddScoped<IMediaService, MediaService>();
            return services;
        }
    }
}