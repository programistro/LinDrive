using LinDrive.Core.Models;

namespace LinDrive.Application.Interfaces;

public interface IAccessTokenService
{
    Task<AccessToken> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<AccessToken> AddAsync(string token, Guid userId, CancellationToken cancellationToken);
    
    Task UpdateAsync(AccessToken token, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    Task<IEnumerable<AccessToken>> GetAllAsync(CancellationToken cancellationToken);
}