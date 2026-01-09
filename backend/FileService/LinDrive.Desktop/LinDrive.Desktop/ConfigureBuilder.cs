using System;
using LinDrive.Application.Services;
using LinDrive.Core.Repositories;
using LinDrive.Desktop.ViewModels;
using LinDrive.Infrastructure.Data;
using LinDrive.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<AppDbContext>(optionsAction: options =>
            {
                options.UseSqlite("Data Source=app.db; Password=123");
            });
            
            return services;
        }

        public IServiceCollection ConfigureRepositories()
        {
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            
            return services;
        }
    }
}