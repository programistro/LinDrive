using LinDrive.Application.Interfaces;
using LinDrive.Contracts.Dtos;
using LinDrive.Contracts.Requestes;
using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;
using LinDrive.Shared;
using LinDrive.Shared.Enums;
using LinDrive.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace LinDrive.Application.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly ICryptoService _cryptoService;
    private readonly IAccessTokenRepository  _accessTokenRepository;

    public AuthService(ILogger<AuthService> logger,
        IUserService userService,
        ITokenService tokenService,
        ICryptoService cryptoService, IAccessTokenRepository accessTokenRepository)
    {
        _logger = logger;
        _userService = userService;
        _tokenService = tokenService;
        _cryptoService = cryptoService;
        _accessTokenRepository = accessTokenRepository;
    }

    public async Task<Result<User>> Authenticate(AuthRequest request, AuthType authType,UserAgent userAgent,
        CancellationToken cancellationToken)
    {
        var find = await _userService.GetByEmailAsync(request.Email, cancellationToken);

        if (authType == AuthType.Register)
        {
            if (find != null)
            {
                if (find.PasswordHash != _cryptoService.CreatePasswordHash(request.Password))
                    return Result<User>.Failure("Passwords do not match");

                return Result<User>.Success(find);
            }
            else
            {
                User user = new()
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    PasswordHash = _cryptoService.CreatePasswordHash(request.Password),
                };
                var refreshToken = await _tokenService.GenerateToken(user, userAgent, cancellationToken);
                user.AccessToken = refreshToken.Token;
                user.AccessTokens.Add(refreshToken.Token);
                await _accessTokenRepository.AddAsync(refreshToken, cancellationToken);
                
                return Result<User>.Success(user);
            }
        }

        if (authType == AuthType.Login)
        {
            if (find != null)
            {
                if (find.PasswordHash != _cryptoService.CreatePasswordHash(request.Password))
                    return Result<User>.Failure("Passwords do not match");

                return Result<User>.Success(find);
            }
        }
        
        return Result<User>.Failure("User not found");
    }
}