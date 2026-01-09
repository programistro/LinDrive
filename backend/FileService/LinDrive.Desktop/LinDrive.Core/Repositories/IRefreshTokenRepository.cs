using LinDrive.Core.Models;

namespace LinDrive.Core.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<RefreshToken?> GetByTokensync(string token, CancellationToken cancellationToken);
    
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    
    Task DeleteAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}