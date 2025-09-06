using LinDrive.Core.Models;

namespace LinDrive.Core.Interfaces;

public interface IAccessTokenRepository
{
    Task<AccessToken?> GetByIdAsync(Guid tokenId, CancellationToken cancellationToken);
    
    Task<AccessToken?> GetByAsync(string token, CancellationToken cancellationToken);
    
    Task AddAsync(AccessToken token, CancellationToken cancellationToken);
    
    Task UpdateAsync(AccessToken token, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid tokenId, CancellationToken cancellationToken);
    
    Task<IEnumerable<AccessToken>> GetAllAsync(CancellationToken cancellationToken);
}