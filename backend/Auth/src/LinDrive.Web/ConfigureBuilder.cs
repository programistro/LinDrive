using FluentValidation;
using LinDrive.Application.Interfaces;
using LinDrive.Application.Services;
using LinDrive.Application.Validators;
using LinDrive.Core.Interfaces;
using LinDrive.Infrastructure.Repositories;
using LinDrive.Shared.Interfaces;
using LinDrive.Shared.Services;

namespace LinDrive.Web;

public static class ConfigureBuilder
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureServices()
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IInfoService, InfoService>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        public IServiceCollection ConfigureRepositories()
        {
            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public IServiceCollection ConfigureValidatos()
        {
            services.AddValidatorsFromAssemblyContaining<AuthValidator>();
        
            return services;
        }
    }
}