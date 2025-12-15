using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LinDrive.Application.Interfaces;
using LinDrive.Application.Results;
using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;
using LinDrive.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LinDrive.Application.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IAccessTokenRepository _accessTokenRepository;
    private readonly IUserService _userService;

    public TokenService(IOptions<JwtOptions> jwtOptions, IAccessTokenRepository accessTokenRepository, IUserService userService)
    {
        _jwtOptions = jwtOptions.Value;
        _accessTokenRepository = accessTokenRepository;
        _userService = userService;
    }

    public async Task<AccessToken> ValidateTokenById(Guid id, CancellationToken cancellationToken)
    {
        var findToken = await _accessTokenRepository.GetByIdAsync(id, cancellationToken);

        if (findToken == null || findToken.ExpiresAt < DateTime.UtcNow)
            return new AccessToken();
        
        return findToken;
    }
    
    public async Task<Result<AccessToken>> ValidateToken(string token, CancellationToken cancellationToken)
    {
        var findToken = await _accessTokenRepository.GetTokenAsync(token, cancellationToken);

        if (findToken == null)
            return Result<AccessToken>.Failure("Token not found");

        if (findToken.ExpiresAt < DateTime.UtcNow)
            return Result<AccessToken>.Failure("Token is expired");
        
        return Result<AccessToken>.Success(findToken);
    }

    public async Task<string> GenerateToken(User user, CancellationToken cancellationToken)
    {
        const int byteLength = 48;
        byte[] randomBytes = new byte[byteLength];
    
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(randomBytes);

        string base64Token = Convert.ToBase64String(randomBytes);
        base64Token = "sk-" + base64Token.TrimEnd('=').Replace('+', '-').Replace('/', '_');

        AccessToken token = new()
        {
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(2),
            Id = Guid.NewGuid(),
            Token = base64Token,
            User = user,
        };
        user.AccessToken = token.Token;
        user.AccessTokens.Add(token);
        await _accessTokenRepository.AddAsync(token, cancellationToken);
        await _userService.UpdateAsync(user, cancellationToken);
    
        return base64Token;
    }

    public Guid? GetUserIdFromToken(string token, CancellationToken cancellationToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (userId == null)
            return null;
        
        return Guid.Parse(userId);
    }

    public async Task DeleteTokenByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var findToken = await _accessTokenRepository.GetByIdAsync(id, cancellationToken);

        if (findToken == null)
            return;

        await _accessTokenRepository.DeleteAsync(findToken.Id, cancellationToken);
    }

    public async Task DeleteTokenAsync(string token, CancellationToken cancellationToken)
    {
        var findToken = await _accessTokenRepository.GetTokenAsync(token, cancellationToken);

        if (findToken == null)
            return;

        await _accessTokenRepository.DeleteAsync(findToken.Id, cancellationToken);
    }

    public async Task<IEnumerable<AccessToken>> GetTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(userId, cancellationToken);

        if (user.AccessTokens.Count() > 0)
            return user.AccessTokens;
        
        return null;
    }

    public string GenerateJwtToken(string email, Guid userId)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
            signingCredentials:creds);
        
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        
        return token;
    }
}