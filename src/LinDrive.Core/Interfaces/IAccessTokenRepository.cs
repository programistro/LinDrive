using LinDrive.Core.Models;
using LinDrive.Shared;

namespace LinDrive.Core.Interfaces;

public interface IAccessTokenRepository
{
    Task<Result<AccessToken>?> GetTokenAsync(string token, CancellationToken cancellationToken);
    
    Task AddAsync(AccessToken token, CancellationToken cancellationToken);
    
    Task UpdateAsync(AccessToken token, CancellationToken cancellationToken);
    
    Task DeleteAsync(string token, CancellationToken cancellationToken);

    Task DisableAllRefreshAsync(List<AccessToken> refreshTokens, CancellationToken cancellationToken,
        string? refreshToken = null);
}