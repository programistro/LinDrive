using LinDrive.Application.Results;
using LinDrive.Core.Models;

namespace LinDrive.Application.Interfaces;

public interface ITokenService
{
    Task<AccessToken> ValidateTokenById(Guid id, CancellationToken cancellationToken);
    
    Task<Result<AccessToken>> ValidateToken(string token, CancellationToken cancellationToken);
    
    Task<string> GenerateToken(User user, CancellationToken cancellationToken);

    string GenerateJwtToken(string email, Guid userId);
    
    Guid? GetUserIdFromToken(string token, CancellationToken cancellationToken);

    Task DeleteTokenByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task DeleteTokenAsync(string token, CancellationToken cancellationToken);

    Task<IEnumerable<AccessToken>> GetTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}