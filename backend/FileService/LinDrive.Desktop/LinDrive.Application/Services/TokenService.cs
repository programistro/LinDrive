using Microsoft.Extensions.Logging;

namespace LinDrive.Application.Services;

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private string AccessToken { get; set; }
    private string RefreshToken { get; set; }

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    public string GetAccessToken() => AccessToken;

    public string GetRefreshToken() => RefreshToken;

    public async Task<string> SetAccessToken(string accessToken)
    {
        return "";
    }

    public async Task<string> SetRefreshToken(string refreshToken)
    {
        return "";
    }
}