using System.Text;
using LinDrive.Application.Interfaces;
using LinDrive.Contracts.Dtos;
using LinDrive.Contracts.Requestes;
using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;
using LinDrive.Shared;
using LinDrive.Shared.Enums;
using LinDrive.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using NBitcoin;
using NBitcoin.DataEncoders;

namespace LinDrive.Application.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly ICryptoService _cryptoService;
    private readonly IAccessTokenRepository _accessTokenRepository;

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

    public async Task<Result<User>> Authenticate(AuthRequest request, UserAgent userAgent,
        CancellationToken cancellationToken)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(request.PubKey);
        if (PubKey.TryCreatePubKey(bytes, out PubKey pubKey))
            return Result<User>.Failure("PubKey not found");

        var computedId = pubKey.Hash.ToString();

        if (computedId != request.UserId)
            Result<User>.Failure("UserId mismatch");

        var find = await _userService.GetByUserIdAsync(request.UserId, cancellationToken);
        var refersh = await _tokenService.GenerateToken(find, userAgent, cancellationToken);
        // var user = await _db.Users.FindAsync(request.UserId) ?? 
        //            await CreateUser(request.UserId, pubKey);
        if (find != null)
        {
            find.AccessToken = refersh.Token;
            find.AccessTokens.Add(refersh.Token);
            await _accessTokenRepository.AddAsync(refersh, cancellationToken);
            await _userService.UpdateAsync(find, cancellationToken);
            return Result<User>.Success(find);
        }
        else if (find == null)
        {
            find = new User()
            {
                Id = Guid.NewGuid(),
                AccessToken = refersh.Token,
                UserId = request.UserId,
            };
            find.AccessTokens.Add(refersh.Token);
            await _accessTokenRepository.AddAsync(refersh, cancellationToken);
            return Result<User>.Success(find);
        }

        return Result<User>.Failure("User not found");
    }
}