using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LinDrive.Application.Interfaces;
using LinDrive.Application.Results;
using LinDrive.Contracts.Dtos;
using LinDrive.Core;
using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;
using LinDrive.Infrastructure;
using LinDrive.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LinDrive.Application.Services;

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly IAccessTokenRepository  _refreshTokenRepository;
    private readonly IUserService _userService;
    private readonly JwtOption _jwtOption;
    private readonly IInfoService  _infoService;

    public TokenService(ILogger<TokenService> logger, IAccessTokenRepository refreshTokenRepository, 
        IUserService userService, IOptions<JwtOption> jwtOption, IInfoService infoService)
    {
        _logger = logger;
        _refreshTokenRepository = refreshTokenRepository;
        _userService = userService;
        _infoService = infoService;
        _jwtOption = jwtOption.Value;
    }

    public async Task<Result<AccessToken>> ValidateToken(string token, CancellationToken cancellationToken)
    {
        var findToken = await _refreshTokenRepository.GetByTokenAsync(token, cancellationToken);

        if (findToken.IsFailure)
            return Result<AccessToken>.Failure(findToken.Error);

        if (findToken.Value.ExpiresAt < DateTime.UtcNow)
            return Result<AccessToken>.Failure("Token is expired");
        
        return Result<AccessToken>.Success(findToken.Value);
    }

    public async Task<AccessToken> GenerateToken(User user, UserAgent agent, CancellationToken cancellationToken)
    {
        const int byteLength = 48;
        byte[] randomBytes = new byte[byteLength];

        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(randomBytes);

        string base64Token = Convert.ToBase64String(randomBytes);
        base64Token = "sk-" + base64Token.TrimEnd('=').Replace('+', '-').Replace('/', '_');

        var location = await _infoService.GetLocation(agent.Ip, cancellationToken);
        
        AccessToken token = new()
        {
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(10),
            Id = Guid.NewGuid(),
            Token = base64Token,
            UserId = user.Id,
            Agent = agent.Agent,
            Location = location,
        };
        user.AccessTokens.Add(token.Token);
        await _refreshTokenRepository.AddAsync(token, cancellationToken);
        await _userService.UpdateAsync(user, cancellationToken);

        return token;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var jwt = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
            signingCredentials:creds);
        
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        
        return token;
    }

    public string? GetUserIdFromToken(string token, CancellationToken cancellationToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var email = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;

        if (email == null)
            return null;
        
        return email;
    }

    public string? GetRoleFromToken(string token, CancellationToken cancellationToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var role = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        
        if (role == null)
            return null;
        
        return role;
    }

    public async Task DeleteTokenAsync(string token, CancellationToken cancellationToken)
    {
        var findToken = await _refreshTokenRepository.GetByTokenAsync(token, cancellationToken);

        if (findToken.IsFailure)
            return;

        await _refreshTokenRepository.DeleteAsync(findToken.Value.Token, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(userId, cancellationToken);

        if (user.AccessToken.Count() > 0)
            return user.AccessTokens;
        
        return null;
    }
}