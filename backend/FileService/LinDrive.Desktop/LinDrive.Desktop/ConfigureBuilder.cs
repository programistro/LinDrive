using System;
using LinDrive.Application.Services;
using LinDrive.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LinDrive.Desktop;

public static class ConfigureBuilder
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureServices()
        {
            services.AddSingleton<ITokenService, TokenService>();
            services.AddTransient<MainWindowViewModel>();
            
            return services;
        }
    }
}